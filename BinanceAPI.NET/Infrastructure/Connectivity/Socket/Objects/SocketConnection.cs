using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinanceAPI.NET.Infrastructure.Interfaces;

namespace BinanceAPI.NET.Infrastructure.Connectivity.Socket.Objects
{
    internal class SocketConnection
    {
        public enum SOCKET_STATE
        {
            Initial,
            Connected,
            Disconnecting,
            Disconnected,
            Disposed
        }

        private SOCKET_STATE State;
        private bool Paused;
        public bool Authenticated { get; internal set; }
        private readonly IWebSocket _Socket;


    }
}
