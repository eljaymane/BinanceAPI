using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Enums
{
    internal enum WebSocketProcessState
    {
        Idle,
        Processing,
        WaitingForClose,
        Reconnecting
    }
}
