using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Enums;
using BinanceAPI.NET.Infrastructure.Interfaces;
using BinanceAPI.NET.Infrastructure.Primitives;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace BinanceAPI.NET.Infrastructure.Connectivity.Socket
{
ary>
    /// This is an implementation of an asynchronous socket client that can connect to a remote host, receive and send data. All the behavioral response to messages is triggered using the delegates OnMessage, OnError...
    /// </summary>
    public class WebSocketClient : IWebSocket
    {
        private ILogger _logger;
        internal static int lastStreamId;
        private static readonly object streamIdLock = new();

        private readonly AsyncAutoResetEvent _sendEvent;
        private readonly ConcurrentQueue<byte[]> _sendBuffer;
        private CancellationTokenSource _ctsSource = new CancellationTokenSource();

        private ClientWebSocket _socket;
        
        private WebSocketProcessState _processState;
        private bool disposed;
        private DateTime lastReceivedPacketsUpdate;

        public int Id { get; }
        public SocketConfiguration Configuration { get; }
        public Uri BaseUri => Configuration.BaseUri;
        public bool IsOpen => _socket.State == WebSocketState.Open && !_ctsSource.IsCancellationRequested;
        public bool IsClosed => _socket.State == WebSocketState.Closed;
        /// <summary>
        /// These events are used to trigger behaviors according to the client events.
        /// </summary>
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
        /// <summary>
        /// The function that generates the websocket object that is used by this client.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// The function that connects to a remote host , starts the send and receive loop then calls OnOpen delegate which is implemented elsewhere.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// The send loop : Keeps sending anything queued to _sendBuffer.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// The receive loop : Keeps receiving and calling HandleMessage to take care of the received data.
        /// </summary>
        /// <returns></returns>
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
                }
            }
        }
        /// <summary>
        /// Removing trailing 0's from received messages.
        /// </summary>
        /// <param name="data">Received bytes</param>
        /// <returns></returns>
        private Task<byte[]> removeZeros(byte[] data)
        {
            var stringData = Configuration.Encoding.GetString(data).Replace("\0","");
            return Task.FromResult(Configuration.Encoding.GetBytes(stringData));
        }
        /// <summary>
        /// Handles every received message, by either closing if it's a close message or calling OnMessage which is implemented elsewhere.
        /// </summary>
        /// <param name="message">The received message</param>
        /// <param name="buffer">The received bytes</param>
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
        /// <summary>
        /// The method used to enqueue messages to be sent on the _sendBuffer.
        /// </summary>
        /// <param name="data">Bytes to be sent</param>
        /// <returns></returns>
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
        /// <summary>
        /// Closes the already connected socket object
        /// </summary>
        /// <returns></returns>
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

        private static int NextStreamId()
        {
            lock(streamIdLock)
            {
                lastStreamId++;
                return lastStreamId;
            }
        }

    }
}
