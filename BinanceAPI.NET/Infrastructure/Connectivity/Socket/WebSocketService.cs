using BinanceAPI.NET.Infrastructure.Abstractions;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Connectivity.Socket
{
    public class WebSocketService<T> : AbstractWebSocketService<T>
    {
        private ILoggerFactory _loggerFactory;

        public event Action<Exception>? OnError { add => OnError += value; remove => OnError -= value; }
        public event Action<dynamic>? OnMessage { add => OnMessage += value;remove => OnMessage -= value; }
        public event Action? OnClose { add => OnClose += value; remove => OnClose -= value; }
        public event Action? OnOpen { add => OnOpen += value; remove => OnOpen -= value; }
        public event Action? OnReconnecting { add => OnReconnecting += value; remove => OnReconnecting -= value; }
        public event Action? OnReconnected { add => OnReconnected += value; remove => OnReconnected -= value; }
        public WebSocketService(SocketConfiguration configuration,  ILoggerFactory loggerFactory, CancellationTokenSource? ctSource = null) : base(ctSource)
        {
            _loggerFactory = loggerFactory;
            Client = new WebSocketClient(loggerFactory.CreateLogger<WebSocketClient>(),configuration,ctSource);
        }

        public override void Start()
        {
            Client.ConnectAsync();
        }

        private Task<T?> Deserialize(byte[] message)
        {
            var obj = JsonSerializer.Deserialize<T>(message);
            return Task.FromResult(obj);
        }

        private Task<byte[]> Serialize(T obj)
        {
            var jsonBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<T>(obj));
            return Task.FromResult(jsonBytes);
        }

        public async override Task SendRequestAsync(T request)
        {
            await Client.SendAsync(new ArraySegment<byte>(Serialize(request).Result));
        }
    }
}
