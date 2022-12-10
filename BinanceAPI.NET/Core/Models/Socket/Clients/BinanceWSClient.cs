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
    public class BinanceWSClient 
    {
        private CancellationTokenSource ctSource;
        private ILoggerFactory loggerFactory;
        private WebSocketClient Connection;
        public BinanceWSConfiguration Configuration;
        public BinanceWSClient(BinanceWSConfiguration configuration, ILoggerFactory _loggerFactory, CancellationTokenSource cancellationTokenSource)
        {
            Configuration = configuration;
            loggerFactory = _loggerFactory;
            ctSource= cancellationTokenSource;
            Connection = new(loggerFactory.CreateLogger<WebSocketClient>(), new SocketConfiguration(Configuration.BaseUri, true),
                ctSource);
        }

        public async Task ConnectAsync()
        {
           await Connection.ConnectAsync();
        }

        public Task SendRequest(BinanceWebSocketRequestMessage request,CancellationToken cancellationToken)
        {
            if(!cancellationToken.IsCancellationRequested)
            {
                var serializeOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters =
    {
        new RequestMessageTypeJsonConverter()
    }
                };
                var data = JsonSerializer.Serialize(request, serializeOptions);
                Connection.Send(data);
            }
            return Task.CompletedTask;  
        }
    }
}
