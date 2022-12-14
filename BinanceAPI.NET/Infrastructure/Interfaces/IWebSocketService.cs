using System.Text.Json;
using BinanceAPI.NET.Core.Interfaces;

namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    public interface IWebSocketService<T> where T : IRequestDataType
    {
        void Start();
        void SendRequestAsync(T request);
        Task<IBinanceResponse<IBinanceStreamData>?> Deserialize(byte[] message);
        Task<byte[]> Serialize(T obj);
        public event Action<Exception>? OnError;
        public event Action<byte[]>? OnMessage;
        public event Action? OnClose;
        public event Action? OnOpen;
        public event Action? OnReconnecting;
        public event Action? OnReconnected;
    }
}