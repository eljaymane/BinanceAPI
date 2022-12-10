using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Streams;
using BinanceBOT;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace BinanceAPI.NET.Core.Models
{
    public class BinanceMarketDataClient
    {
        public ILoggerFactory LoggerFactory { get; set; }
        private BinanceKlineStream? KlineStream { get; set; }
        private BinanceIndividualSymbolTickerStream? SymbolTickerStream { get; set; }

        private string Symbol;

        public CancellationTokenSource TokenSource { get; set; }

        public BinanceMarketDataClient(string symbol, CancellationTokenSource tokenSource)
        {
            Symbol = symbol;
            TokenSource = tokenSource;
        }

        public void SubscribeToKlineStreamAsync(KlineInterval interval = KlineInterval.FifteenMinutes)
        {
            KlineStream = new BinanceKlineStream(interval, Symbol, TokenSource,LoggerFactory);
            KlineStream = KlineStream.SetLoggerFactory(LoggerFactory);
            KlineStream.SubscribeAsync();
        }

    }
}