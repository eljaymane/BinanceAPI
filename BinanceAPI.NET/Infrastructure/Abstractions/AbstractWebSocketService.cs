using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Abstractions
{
    public abstract class AbstractWebSocketService<T> : IWebSocketService<T>
    {
        internal IWebSocket Client;

        public CancellationTokenSource? CTSource { get; set; }

        public AbstractWebSocketService(CancellationTokenSource? ctSource = null)
        {
            CTSource = ctSource == null ? new CancellationTokenSource() : ctSource;
        }

        public abstract void Start();
        public abstract Task SendRequestAsync(T request);
    }
}
