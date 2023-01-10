using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Enums
{
    /// <summary>
    /// The different states the WebSocket client can have.
    /// </summary>
    internal enum WebSocketProcessState
    {
        Idle,
        Processing,
        WaitingForClose,
        Reconnecting
    }
}
