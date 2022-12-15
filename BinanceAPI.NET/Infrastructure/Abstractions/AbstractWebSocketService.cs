using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Interfaces;

namespace BinanceAPI.NET.Infrastructure.Abstractions
{
    public abstract class AbstractWebSocketService<T> : IWebSocketService<T> where T : IRequestDataType
    {
        internal IWebSocket Client;

        public abstract event Action<Exception>? OnError;
        public abstract event Action<byte[]>? OnMessage;
        public abstract event Action? OnClose;
        public abstract event Action? OnOpen;
        public abstract event Action? OnReconnecting;
        public abstract event Action? OnReconnected;

        public CancellationTokenSource? CTSource { get; set; }

        public AbstractWebSocketService(CancellationTokenSource? ctSource = null)
        {
            CTSource = ctSource == null ? new CancellationTokenSource() : ctSource;
        }

        public abstract void Start();
        public abstract void SendRequestAsync(T request);
        public abstract Task<IBinanceResponse<IBinanceStreamData>?> Deserialize(byte[] message);
        public abstract Task<byte[]> Serialize(T obj);

    }
}
