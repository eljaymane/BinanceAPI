using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    public interface IWebSocket
    {
        public event Action<Exception>? OnError { add => OnError += value;  remove => OnError -= value; }
        public event Action<string>? OnReceive { add => OnReceive += value; remove => OnReceive -= value; }
        public event Action? OnClose { add => OnClose += value; remove => OnClose -= value; }
        public event Action? OnOpen { add => OnOpen += value; remove => OnOpen -= value; }
        public event Action? OnReconnecting { add => OnReconnecting += value; remove => OnReconnecting -= value; }
        public event Action? OnReconnected { add => OnReconnected += value; remove => OnReconnected -= value; }

        Task ConnectAsync();

        Task SendAsync(ArraySegment<byte> data);

    }
}
