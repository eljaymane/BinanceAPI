using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Infrastructure.Abstractions;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace BinanceAPI.NET.Infrastructure.Connectivity.Socket
{
    /// <summary>
    /// Wrapper around the core socket client that adds requests message handling. In the context of binance, the same request message format will be used to perform action against their server.
    /// This service is meant to be used in the business layer when a socket connection is needed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WebSocketService<T> : AbstractWebSocketService<T> where T : IRequestDataType
    {
        private ILoggerFactory _loggerFactory;

        public override event Action<Exception>? OnError;
        public override event Action<byte[]>? OnMessage;
        public override event Action? OnClose;
        public override event Action? OnOpen;
        public override event Action? OnReconnecting;
        public override event Action? OnReconnected;

        public WebSocketService(SocketConfiguration configuration,  ILoggerFactory loggerFactory, CancellationTokenSource? ctSource = null) : base(ctSource)
        {
            _loggerFactory = loggerFactory;
            Client = new WebSocketClient(loggerFactory.CreateLogger<WebSocketClient>(),configuration,ctSource);
        }

        /// <summary>
        /// This method links local delegates to the client's one and connects the socket to the remote server.
        /// </summary>
        public override void Start()
        {
            Client.OnError += OnError;
            Client.OnClose += OnClose;
            Client.OnReconnecting += OnReconnecting;
            Client.OnReconnected += OnReconnected;
            Client.OnMessage += OnMessage;
            Client.ConnectAsync().Wait();
        }

     

        public async override void SendRequestAsync(T request)
        {
            await Client.SendAsync(new ArraySegment<byte>(Serialize(request).Result));
        }

        /// <summary>
        /// The method that serializes the request and returns bytes to be sent using the WebSocketClient.
        /// </summary>
        /// <param name="obj">The request of type T to be serialized.</param>
        /// <returns></returns>
        public override Task<byte[]> Serialize(T obj)
        {
            var json = JsonConvert.SerializeObject(obj, IRequestDataType.GetSerialiazationSettings());
            return Task.FromResult(Encoding.UTF8.GetBytes(json));
        }


    }
}
