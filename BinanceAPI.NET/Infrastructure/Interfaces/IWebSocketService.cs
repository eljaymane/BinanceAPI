using System.Text.Json;
using BinanceAPI.NET.Core.Interfaces;

namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    /// <summary>
    /// WebSocketService contract of a websocket client capable of sending a request T and acting based on the events described in the contract.
    /// </summary>
    /// <typeparam name="T">Contract of the request data type to be sent.</typeparam>
    public interface IWebSocketService<T> where T : IRequestDataType
    {
        void Start();
        Task SendRequestAsync(T request);
        Task<byte[]> Serialize(T obj);
        public event Action<Exception>? OnError;
        public event Action<byte[]>? OnMessage;
        public event Action? OnClose;
        public event Action? OnOpen;
        public event Action? OnReconnecting;
        public event Action? OnReconnected;
    }
}