﻿using BinanceAPI.NET.Core.Primitives;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Enums;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
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
        private SemaphoreSlim sendBufferSem = new(1,1);
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
            //var cookieContainer = new CookieContainer();
            //foreach (var cookie in Configuration.Cookies)
            //{
            //    cookieContainer.Add(new Cookie(cookie.Key, cookie.Value));
            //}
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

               _socket.ConnectAsync(new Uri("wss://stream.binance.com/ws"), _ctsSource.Token).Wait(_ctsSource.Token);
            var t1 = new Thread(async () => { await StartSendingAsync().ConfigureAwait(false); });
            t1.Start();
            
                var t2 = new Thread( async () => {await  StartReceivingAsync().ConfigureAwait(false); });
                t2.Start();
                
            OnOpen?.Invoke();
            
        }

        private async Task StartSendingAsync()
        {
           
            while (true)
            {

               _sendEvent.WaitAsync();
                

                    _logger.LogInformation("Sending");
                    
                    if(_sendBuffer.TryDequeue(out var data))
                    {
                        await _socket.SendAsync(data, WebSocketMessageType.Text, true, _ctsSource.Token).ConfigureAwait(false);
                    }
                
            }
           

        }

        private async Task StartReceivingAsync()
        {
           while (!_ctsSource.IsCancellationRequested)
            {
                //_sendEvent.WaitAsync();
                //_logger.LogInformation("Receiving");
                var buffer= new byte[Configuration.SOCKET_BUFFER_SIZE];
                var result = await _socket.ReceiveAsync(buffer,_ctsSource.Token).ConfigureAwait(false);
                string json = Configuration.Encoding.GetString(buffer);
               
            }
        }

        private void HandleMessage(byte[] buffer, int offset, int count,WebSocketMessageType messageType)
        {
           
        }

        public virtual void Send(string data)
        {
            if (_ctsSource.IsCancellationRequested) return;
            var bytes = Encoding.UTF8.GetBytes(data);
            lock (sendBufferSem)
            {
                _sendBuffer.Enqueue(bytes);
                _sendEvent.Set();
            }
           
           
            
            

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
