using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Socket;
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
        public KlineInterval Interval { get; set; }

        public BinanceKlineStream(KlineInterval interval, string symbol,CancellationTokenSource tokenSource) : base(symbol,BinanceStreamType.KlineCandlestick,tokenSource)
        {
            Interval = interval;
            Name = Name + interval.GetStringValue();
            
        }

        public async void SubscribeAsync()
        {
            Client = new WebSocketClient(_loggerFactory.CreateLogger<BinanceKlineStream>(), new SocketConfiguration(new Uri("wss://stream.binance.com:9443/"), true), TokenSource);
            await Client.ConnectAsync();
        }

        public BinanceKlineStream SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            return this;
        }
    }
}
