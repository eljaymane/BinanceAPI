using BinanceAPI.NET.Core.Primitives;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Enums;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Connectivity.Socket
{
    public struct ReceivedPacket
    {
        internal DateTime ReceivedAt;
        internal int Bytes { get; set; }

        public ReceivedPacket(DateTime receivedAt, int bytes)
        {
            ReceivedAt = receivedAt;
            Bytes = bytes;
        }
    }
    public class WebSocketClient : IWebSocket
    {
        private ILogger _logger;
        internal static int lastStreamId;
        private static readonly object streamIdLock = new();

        private readonly AsyncAutoResetEvent _sendEvent;
        private readonly ConcurrentQueue<byte[]> _sendBuffer;
        private readonly SemaphoreSlim _closeSem;
        public  List<ReceivedPacket> _receivedPackets;
        private readonly object _receivedPacketsLock;
        private readonly List<DateTime> _sentPackets;
        private CancellationTokenSource _ctsSource = new CancellationTokenSource();

        private ClientWebSocket _socket;
        
        private WebSocketProcessState _processState;
        private WebSocketState _socketState;
        private Task? _processTask;
        private Task? _closeTask;
        private bool isStopRequested;
        private bool disposed;
        private DateTime lastReconnectTime;
        private DateTime lastReceivedPacketsUpdate;

        public int Id { get; }
        public SocketConfiguration Configuration { get; }
        public DateTime LastActionTime { get; private set; }
        public Uri BaseUri => Configuration.BaseUri;
        public bool IsOpen => _socket.State == WebSocketState.Open && !_ctsSource.IsCancellationRequested;
        public bool IsClosed => _socket.State == WebSocketState.Closed;

        public event Action<Exception>? OnError;
        public event Action<string>? OnReceive;
        public event Action? OnClose;
        public event Action? OnOpen;
        public event Action? OnReconnecting;
        public event Action? OnReconnected;

        public WebSocketClient(ILogger logger,SocketConfiguration configuration,CancellationTokenSource ctsSource)
        {
            Id = NextStreamId();
            _logger = logger;
            Configuration = configuration;
            _ctsSource = ctsSource;
            _sentPackets = new();
            _receivedPackets = new();
            _sendEvent = new();
            _sendBuffer = new();
            _receivedPacketsLock = new();
            _closeSem = new(1, 1);
            _socket = CreateSocket();
        }

        private ClientWebSocket CreateSocket()
        {
            var cookieContainer = new CookieContainer();
            foreach (var cookie in Configuration.Cookies)
            {
                cookieContainer.Add(new Cookie(cookie.Key, cookie.Value));
            }
            var socket = new ClientWebSocket();
            try
            {
                socket.Options.Cookies = cookieContainer;
                foreach (var header in Configuration.Headers)
                {
                    socket.Options.SetRequestHeader(header.Key, header.Value);
                }
                socket.Options.KeepAliveInterval = Configuration.KeepAliveInterval ?? TimeSpan.Zero;
                socket.Options.SetBuffer(Configuration.SOCKET_BUFFER_SIZE, Configuration.SOCKET_BUFFER_SIZE);
                
            }catch(PlatformNotSupportedException) { }
            return socket;
        }

        public virtual async Task<bool> ConnectAsync()
        {
            if(!await ConnectInternalAsync().ConfigureAwait(false))
                return false;

            OnOpen?.Invoke();
            _processTask = ProcessAsync();
            return true;
            
        }

        private async Task ProcessAsync()
        {
            while(!isStopRequested)
            {
                _processState = WebSocketProcessState.WaitingForClose;
                var sendLoop = StartSendingAsync();
                var receiveLoop = StartReceivingAsync();
                var timeout = Configuration.Timeout;
            }
        }

        private async Task StartSendingAsync()
        {
            try
            {
                while (!_ctsSource.IsCancellationRequested)
                {
                    await _sendEvent.WaitAsync().ConfigureAwait(false);
                    
                    while(_sendBuffer.TryDequeue(out var data))
                    {
                        if(Configuration.PacketPerSecond != null)
                        {
                            DateTime? start = null;
                            while(MessagesSentLastSecond() >= Configuration.PacketPerSecond)
                            {
                                start ??= DateTime.UtcNow;
                                await Task.Delay(50).ConfigureAwait(false);
                            }
                            if (start != null)
                            {
                                _logger.LogInformation($"Socket {Id} send delayed {Math.Round((DateTime.UtcNow - start.Value).TotalMilliseconds)}ms [RATE LIMIT REACHED]");
                            }
                        }

                        try
                        {
                            await _socket.SendAsync(new ArraySegment<byte>(data,0,data.Length),
                                WebSocketMessageType.Text,true,_ctsSource.Token);
                        }catch(OperationCanceledException) { break; }
                        catch (Exception e) 
                        { 
                            OnError?.Invoke(e); 
                            if(_closeTask?.IsCompleted != false)
                            {
                                _closeTask = CloseInternalAsync();
                            }
                            break;
                        }
                    }
                }
            } catch(Exception e)
            {
                _logger.LogError($"Socker {Id} send thread has thrown an exception. Stopping ..");
                OnError?.Invoke(e);
                throw;
            }
            finally
            {
                _logger.LogInformation($"Socket {Id} Finished sending");
            }

        }

        private async Task StartReceivingAsync()
        {
            var buffer = new ArraySegment<byte>(new byte[Configuration.SOCKET_BUFFER_SIZE]);
            var received = 0;
            while(!_ctsSource.IsCancellationRequested) 
            {
                MemoryStream? ms = null;
                WebSocketReceiveResult? result;
                bool multiPartMessage = false;

                while (true)
                {
                    try
                    {
                        result = await _socket.ReceiveAsync(buffer, _ctsSource.Token).ConfigureAwait(false);
                        received += 0;
                        lock(_receivedPacketsLock)
                        {
                            _receivedPackets.Add(new ReceivedPacket(DateTime.UtcNow, result.Count));
                        }
                    }catch(Exception e)
                    {
                        OnError?.Invoke(e);
                        if(_closeTask?.IsCompleted != false)
                        {
                            _closeTask = CloseInternalAsync();
                        }
                        break;
                    }

                    if(result.MessageType == WebSocketMessageType.Close)
                    {
                        _logger.LogInformation($"Socket {Id} received a close message.");
                        if(_closeTask.IsCompleted!= false)
                        {
                            _closeTask = CloseInternalAsync();
                            break;
                        }
                    }

                    if (!result.EndOfMessage)
                    {
                        multiPartMessage= true;
                        ms ??= new MemoryStream();
                        _logger.LogInformation($"Socket {Id} received {result.Count} of multipart message.");
                        await ms.WriteAsync(buffer.Array, buffer.Offset, result.Count).ConfigureAwait(false);
                    } else
                    {
                        if (!multiPartMessage)
                        {
                            _logger.LogInformation($"Socket {Id} received {result.Count} bytes");
                            HandleMessage(buffer.Array!, buffer.Offset, result.Count, result.MessageType);
                        } else
                        {
                            _logger.LogInformation($"Socket {Id} received {result.Count} bytes of multipart message.");
                            await ms!.WriteAsync(buffer.Array,buffer.Offset, result.Count).ConfigureAwait(false);
                        }
                        break;
                    }
                    if (result == null || _ctsSource.IsCancellationRequested)
                    {
                        break;
                    }

                    lock (_receivedPacketsLock)
                    {
                        UpdateReceivedPackets();
                    }
                        

                    if (result?.MessageType == WebSocketMessageType.Close)
                    {
                        // Received close message
                        break;
                    }

                    if (multiPartMessage)
                    {
                        if(result.EndOfMessage == true)
                        {
                            _logger.LogInformation($"Socket {Id} reassembled message of {ms!.Length} bytes");
                            HandleMessage(ms!.ToArray(), 0, (int)ms.Length, result.MessageType);
                            ms.Dispose();
                        } else
                        {
                            _logger.LogInformation($"Socket {Id} discarding incomplete multi part data of {ms.Length} bytes");
                        }
                    }
                }
            }
        }

        private void HandleMessage(byte[] buffer, int offset, int count,WebSocketMessageType messageType)
        {
            string strData;
            if (messageType == WebSocketMessageType.Binary)
            {
                if(Configuration.DataInterpreterBytes == null) throw new Exception("Could not interprete received byte data");
                try
                {
                    var data = new byte[count];
                    Array.Copy(buffer, offset, data, 0, count);
                    strData = Configuration.DataInterpreterBytes(data);
                } catch(Exception e) 
                {
                    _logger.LogError($"Socket {Id} threw an exception while reading bytes data");
                    return; 
                }
            }else
            {
                strData = Configuration.Encoding.GetString(buffer,offset, count);

                if(Configuration.DataInterpreterString!= null)
                {
                    try
                    {
                        strData = Configuration.DataInterpreterString(strData);
                    } catch(Exception e)
                    {
                        _logger.LogError($"Socket {Id} threw an exception while reading string data");
                        return;
                    }
                }

                try
                {
                    InvokeOnReceive(strData);
                }catch(Exception e)
                {
                    _logger.LogError($"Socket {Id} threw an exception while processing the message");
                }
            }
        }

        public virtual void Send(string data)
        {
            if (_ctsSource.IsCancellationRequested) return;
            var bytes = Encoding.UTF8.GetBytes(data);
            _sendBuffer.Enqueue(bytes);
            _sendEvent.Set();

        }

        private async Task<bool> ConnectInternalAsync()
        {
            try
            {
                using CancellationTokenSource tcs = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await _socket.ConnectAsync(BaseUri, tcs.Token);

            }catch(Exception ex) 
            {
                return false;
            }

            return true;
        }

        private async Task CloseInternalAsync()
        {
            if(disposed) return;
            _ctsSource.Cancel();
            _sendEvent.Set();
            if(_socket.State == WebSocketState.Open)
            {
                try
                {
                    await _socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closing", default).ConfigureAwait(false);
                }
                catch(Exception) { }
            }
            else if(_socket.State == WebSocketState.CloseReceived)
            {
                try
                {
                    await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure,"Closing",default).ConfigureAwait(false);
                }catch(Exception) { }
            }
        }

        public virtual async Task ReconnectAsync()
        {
            if (_processState != WebSocketProcessState.Processing && IsOpen)
                return;
            _closeTask = CloseInternalAsync();
            await _closeTask.ConfigureAwait(false);
        }

        private static int NextStreamId()
        {
            lock(streamIdLock)
            {
                lastStreamId++;
                return lastStreamId;
            }
        }

        private int MessagesSentLastSecond()
        {
            var time = DateTime.UtcNow;
            _sentPackets.RemoveAll(e => time - e > TimeSpan.FromSeconds(1));
            return _sentPackets.Count;
        }

        protected void InvokeOnReceive(string data)
        {
            LastActionTime = DateTime.UtcNow;
            OnReceive.Invoke(data);
        }

        protected void InvokeOnError(Exception e) => OnError?.Invoke(e);

        protected void InvokeOnClose() => OnClose?.Invoke();
        protected void InvokeOnReconnecting() => OnReconnecting?.Invoke();
        protected void InvokeOnReconnected() => OnReconnected?.Invoke();
        protected async Task CheckTimeoutAsync()
        {
            try
            {
                while(!_ctsSource.IsCancellationRequested) 
                {
                    if(DateTime.UtcNow - LastActionTime > Configuration.Timeout)
                    {
                        _logger.LogInformation($"Socket {Id} : Didn't receive data for {Configuration.Timeout}. Reconnecting...");
                        _ = ReconnectAsync().ConfigureAwait(false);
                        return;
                    }
                    try
                    {
                        await Task.Delay(500,_ctsSource.Token).ConfigureAwait(false);
                    } catch(OperationCanceledException) { break; }
                }
            }catch(Exception e)
            {
                InvokeOnError(e);
                throw;
            }
        }

        protected void UpdateReceivedPackets()
        {
            var checkTime = DateTime.UtcNow;
        if (checkTime - lastReceivedPacketsUpdate > TimeSpan.FromSeconds(1))
            {
                foreach (var packet in _receivedPackets)
                {
                    if (checkTime - packet.ReceivedAt > TimeSpan.FromSeconds(3))
                        _receivedPackets.Remove(packet);

                    lastReceivedPacketsUpdate= checkTime;
                }
            }
                
                    
        }
    }
}
