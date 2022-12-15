using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Enums;
using BinanceAPI.NET.Infrastructure.Interfaces;
using BinanceAPI.NET.Infrastructure.Primitives;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net.WebSockets;

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
        public  List<ReceivedPacket> _receivedPackets;
        private readonly List<DateTime> _sentPackets;
        private CancellationTokenSource _ctsSource = new CancellationTokenSource();

        private ClientWebSocket _socket;
        
        private WebSocketProcessState _processState;
        private Task? _closeTask;
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
        public event Action<byte[]>? OnMessage;
        public event Action? OnClose;
        public event Action? OnOpen;
        public event Action? OnReconnecting;
        public event Action? OnReconnected;

        public WebSocketClient(ILogger logger,SocketConfiguration configuration,CancellationTokenSource? ctsSource = null)
        {
            Id = NextStreamId();
            _logger = logger;
            Configuration = configuration;
            _ctsSource = ctsSource == null ? new CancellationTokenSource() : ctsSource;
            _sentPackets = new();
            _receivedPackets = new();
            _sendEvent = new();
            _sendBuffer = new();
            _socket = CreateSocket();
        }

        private ClientWebSocket CreateSocket()
        {
            var socket = new ClientWebSocket();
            try
            {
                foreach (var header in Configuration.Headers)
                {
                    socket.Options.SetRequestHeader(header.Key, header.Value);
                }
                socket.Options.KeepAliveInterval = Configuration.KeepAliveInterval ?? TimeSpan.Zero;
                socket.Options.SetBuffer(Configuration.SOCKET_BUFFER_SIZE, Configuration.SOCKET_BUFFER_SIZE);
                
            }catch(PlatformNotSupportedException) { }
            return socket;
        }

        public virtual async Task ConnectAsync()
        {
            CancellationTokenSource cts = new(TimeSpan.FromSeconds(5));
            _socket.ConnectAsync(BaseUri, _ctsSource.Token).Wait(_ctsSource.Token);
            if(_socket.State!= WebSocketState.Open) { _ctsSource.Cancel(); return; }
            
            var t2 = new Thread( async () => { await StartReceivingAsync(); });
            var t1 = new Thread(async () => { await StartSendingAsync(); });
            t1.Start();
            t2.Start();
       

            OnOpen?.Invoke();  
        }

        private async Task StartSendingAsync()
        {
           
            while (true)
            {
               await _sendEvent.WaitAsync();
                if(_sendBuffer.TryDequeue(out var data))
                {
                _logger.LogInformation("Sending");
                await _socket.SendAsync(data, WebSocketMessageType.Text, true, _ctsSource.Token);
                }
            }
        }

        private async Task StartReceivingAsync()
        {
            var buffer= new ArraySegment<byte>(new byte[Configuration.SOCKET_BUFFER_SIZE]);
           while (!_ctsSource.IsCancellationRequested)
            {
                try
                {
                    var result = await _socket.ReceiveAsync(buffer,CancellationToken.None).ConfigureAwait(false);
                    HandleMessage(result, await removeZeros(buffer.Array));
                }catch(Exception e)
                {
                    _logger.LogError(e.Message);
                }
                finally
                {
                    buffer = new(new byte[Configuration.SOCKET_BUFFER_SIZE]);
                    Thread.Sleep(1000);
                }
                
                
               
                //string json = Configuration.Encoding.GetString(buffer).Replace("\0","");
                //dynamic d = JsonSerializer.Deserialize(json,typeof(object));
            }
        }

        private Task<byte[]> removeZeros(byte[] data)
        {
            var stringData = Configuration.Encoding.GetString(data).Replace("\0","");
            return Task.FromResult(Configuration.Encoding.GetBytes(stringData));
        }

        private void HandleMessage(WebSocketReceiveResult message, byte[] buffer)
        {
            if (message.MessageType == WebSocketMessageType.Close)
            {
                CloseInternalAsync().Wait();
                _ctsSource.Cancel();
                OnClose?.Invoke();
            } else
            {
                OnMessage?.Invoke(buffer);
            }
        }

        public virtual Task SendAsync(ArraySegment<byte> data)
        {
            if (_ctsSource.IsCancellationRequested) return Task.FromCanceled(_ctsSource.Token);
            lock (_sendBuffer)
            {
                _sendBuffer.Enqueue(data.Array!);
                _sendEvent.Set();
            }
            return Task.CompletedTask;
        }
        private async Task CloseInternalAsync()
        {
            if(disposed) return;
            _ctsSource.Cancel();
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

        protected void InvokeOnReceive(byte[] data)
        {
            LastActionTime = DateTime.UtcNow;
            OnMessage?.Invoke(data);
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
