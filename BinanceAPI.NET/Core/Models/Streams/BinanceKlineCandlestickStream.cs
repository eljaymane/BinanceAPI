using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;

namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceKlineCandlestickStream : AbstractBinanceStream<BinanceWebSocketResponseMessage<BinanceKlineCandlestickData>>
    {
        public BinanceKlineCandlestickStream(ILoggerFactory loggerFactory, SocketConfiguration configuration, CancellationTokenSource tokenSource) : base(BinanceStreamType.KlineCandlestick, configuration, loggerFactory, tokenSource)
        {
            Initialize();
        }

        public void SubscribeAsync(KlineInterval interval, string symbol)
        {
            var request = new BinanceWebSocketRequestMessage(0,
                BinanceRequestMessageType.Subscribe, new string[] { symbol.ToLower() + StreamType.GetStringValue() + interval.GetStringValue() });
            Client.SendRequestAsync(request);
        }

        public Task<BinanceKlineCandlestickData> GetKlineDataAsync()
        {
            //dataSem.Wait();
            return Task.FromResult((BinanceKlineCandlestickData)data);
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

        public override void OnError(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override void OnClose()
        {
            throw new NotImplementedException();
        }

        public override void OnOpen()
        {
            throw new NotImplementedException();
        }

        public override void OnMessage(byte[] streamData)
        {
            var serializerOptions = new JsonSerializerSettings
            {
                Converters = { new KlineIntervalJsonConverter(), new UnixTimestampDateConverter() },
            };
            data = Deserialize(streamData, serializerOptions).Result?.Data;
        }

        public override void OnReconnecting()
        {
            throw new NotImplementedException();
        }

        public override void OnReconnected()
        {
            throw new NotImplementedException();
        }
    }
}
