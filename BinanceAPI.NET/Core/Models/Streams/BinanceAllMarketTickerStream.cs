using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceAllMarketTickerStream : AbstractBinanceStream<BinanceWebSocketResponseMessage<BinanceTickerData>>
    {
        public BinanceAllMarketTickerStream(SocketConfiguration configuration, ILoggerFactory loggerFactory, CancellationTokenSource ctSource) : base(BinanceStreamType.AllMarketTicker,configuration,loggerFactory,ctSource)
        {
            Initialize();
        }
        public void SubscribeAsync()
        {
            var request = new BinanceWebSocketRequestMessage(0,
                BinanceRequestMessageType.Subscribe, new string[] { StreamType.GetStringValue()! });
            var serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new RequestMessageTypeJsonConverter()
                }
            };
            Client.SendRequestAsync(request, serializerOptions);
        }
        public override void Initialize()
        {
            Client.OnError += OnError;
            Client.OnClose += OnClose;
            Client.OnReconnected += OnReconnected;
            Client.OnReconnecting += OnReconnecting;
            Client.OnMessage += OnMessage;
            Client.OnOpen += OnOpen;
            Client.Start();
        }

        public override void OnClose()
        {
            throw new NotImplementedException();
        }

        public override void OnError(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override void OnMessage(byte[] streamData)
        {
            throw new NotImplementedException();
        }

        public override void OnOpen()
        {
            throw new NotImplementedException();
        }

        public override void OnReconnected()
        {
            throw new NotImplementedException();
        }

        public override void OnReconnecting()
        {
            throw new NotImplementedException();
        }
    }
}
