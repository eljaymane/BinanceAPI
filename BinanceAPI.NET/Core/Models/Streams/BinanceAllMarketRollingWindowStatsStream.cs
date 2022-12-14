using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;


namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceAllMarketRollingWindowStatsStream : AbstractBinanceStream<BinanceWebSocketResponseMessage<BinanceRollingWindowStatsData>>
    {
        public BinanceAllMarketRollingWindowStatsStream(SocketConfiguration configuration, ILoggerFactory loggerFactory, CancellationTokenSource ctSource) : base(BinanceStreamType.AllMarketRollingWindowStats, configuration, loggerFactory, ctSource)
        {
            Initialize();
        }

        public void SubscribeAsync(BinanceStatisticsRollingWindowSize windowSize)
        {
            var request = new BinanceWebSocketRequestMessage(0,
              BinanceRequestMessageType.Subscribe, new string[] { StreamType.GetStringValue()!.Replace("<param>", windowSize.GetStringValue()) }) ;
           
            Client.SendRequestAsync(request);
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
            base.OnMessage(streamData);
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
