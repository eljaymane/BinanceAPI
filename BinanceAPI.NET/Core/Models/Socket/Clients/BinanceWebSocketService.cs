using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Socket.Clients
{
    public class BinanceWebSocketService : WebSocketService<BinanceWebSocketRequestMessage>
    {
        private CancellationTokenSource ctSource;
        private ILoggerFactory loggerFactory;
        private WebSocketClient Connection;
        public SocketConfiguration Configuration;
        public BinanceWebSocketService(SocketConfiguration configuration, ILoggerFactory _loggerFactory, CancellationTokenSource cancellationTokenSource) : base(configuration,_loggerFactory,cancellationTokenSource)
        {
            Configuration = configuration;
            loggerFactory = _loggerFactory;
            ctSource= cancellationTokenSource;
            Connection = new(loggerFactory.CreateLogger<WebSocketClient>(), configuration,
                ctSource);
        }

        public async Task ConnectAsync()
        {
           await Connection.ConnectAsync();
        }

   
    }
}
