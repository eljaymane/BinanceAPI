namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    public interface IWebSocketService<T>
    {
        void Start();
        void SendRequestAsync(T request);
        public event Action<Exception>? OnError { add => OnError += value; remove => OnError -= value; }
        public event Action<dynamic>? OnMessage { add => OnMessage += value; remove => OnMessage -= value; }
        public event Action? OnClose { add => OnClose += value; remove => OnClose -= value; }
        public event Action? OnOpen { add => OnOpen += value; remove => OnOpen -= value; }
        public event Action? OnReconnecting { add => OnReconnecting += value; remove => OnReconnecting -= value; }
        public event Action? OnReconnected { add => OnReconnected += value; remove => OnReconnected -= value; }
    }
}