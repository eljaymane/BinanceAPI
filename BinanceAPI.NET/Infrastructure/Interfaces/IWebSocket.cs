using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    public interface IWebSocket
    {
        public event Action<Exception>? OnError;
        public event Action<string>? OnReceive;
        public event Action? OnClose;
        public event Action? OnOpen;
        public event Action? OnReconnecting;
        public event Action? OnReconnected;

        Task<bool> ConnectAsync();

    }
}
