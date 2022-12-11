using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Streams;
using BinanceAPI.NET.Core.Models.Streams.KlineCandlestick;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace BinanceAPI.NET.Core.Models
{
    public class BinanceMarketDataClient
    {
        public ILoggerFactory LoggerFactory { get; set; }
        private BinanceKlineStreamService? KlineStream { get; set; }
        private BinanceIndividualSymbolTickerStream? SymbolTickerStream { get; set; }

        public CancellationTokenSource TokenSource { get; set; }

        public BinanceMarketDataClient(ILoggerFactory loggerFactory,SocketConfiguration socketConfiguration, CancellationTokenSource tokenSource)
        {
            TokenSource = tokenSource;
            KlineStream = new BinanceKlineStreamService(loggerFactory, socketConfiguration,tokenSource);
        }

        public void SubscribeToKlineStreamAsync(string symbol,KlineInterval interval = KlineInterval.FifteenMinutes)
        {
             KlineStream?.SubscribeAsync(interval,symbol);
        }

    }
}