using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Socket;
using BinanceAPI.NET.Core.Models.Socket.Clients;
using BinanceAPI.NET.Infrastructure.Abstractions;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace BinanceBOT
{
    public class BinanceKlineStream : AbstractBinanceStream
    {
        private ILoggerFactory _loggerFactory;
        private CancellationTokenSource ctSource;
        public KlineInterval Interval { get; set; }

        private BinanceWSClient Connection;

        public BinanceKlineStream(KlineInterval interval, string symbol,CancellationTokenSource tokenSource,ILoggerFactory loggerFactory) : base(symbol,BinanceStreamType.KlineCandlestick,tokenSource)
        {
            Interval = interval;
            Name = Name + interval.GetStringValue();
            _loggerFactory= loggerFactory;
            ctSource= tokenSource;
            Connection = new(new BinanceWSConfiguration(new Uri("wss://stream.binance.com:9443/stream?streams=" + Name)),_loggerFactory,ctSource);
        }

        public async void SubscribeAsync()
        {
            Client = new WebSocketClient(_loggerFactory.CreateLogger<BinanceKlineStream>(), new SocketConfiguration(new Uri("wss://stream.binance.com:9443/ws/stream?streams=" + Name), true), TokenSource);
            //await Client.ConnectAsync();
            var request = new BinanceWebSocketRequestMessage(0, BinanceRequestMessageType.Subscribe, new string[]{ Name });
            await Connection.ConnectAsync();
            await Connection.SendRequest(request, ctSource.Token);
        }

        public BinanceKlineStream SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            return this;
        }
    }
}
