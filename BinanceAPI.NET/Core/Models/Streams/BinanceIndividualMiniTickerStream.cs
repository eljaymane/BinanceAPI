using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Core.Models.Socket;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceIndividualMiniTickerStream : AbstractBinanceStream<BinanceWebSocketResponseMessage<BinanceMiniTickerData>>
    {
        public BinanceIndividualMiniTickerStream(SocketConfiguration configuration, ILoggerFactory loggerFactory, CancellationTokenSource tokenSource) : base(BinanceStreamType.IndividualSymbolTicker, configuration, loggerFactory, tokenSource)
        {
                
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
            var serializerOptions = new JsonSerializerSettings
            {
                Converters = { new UnixTimestampDateConverter() },
            };
            data = Deserialize(streamData, serializerOptions).Result?.Data;
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
