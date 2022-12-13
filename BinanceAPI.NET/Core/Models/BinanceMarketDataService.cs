using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Streams;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace BinanceAPI.NET.Core.Models
{
    public class BinanceMarketDataService
    {
        public ILoggerFactory LoggerFactory { get; set; }
        public BinanceKlineCandlestickStream? KlineStream { get; set; }
        private BinanceIndividualSymbolTickerStream? SymbolTickerStream { get; set; }

        public CancellationTokenSource TokenSource { get; set; }

        public BinanceMarketDataService(ILoggerFactory loggerFactory,SocketConfiguration socketConfiguration, CancellationTokenSource tokenSource)
        {
            TokenSource = tokenSource;
            KlineStream = new BinanceKlineCandlestickStream(loggerFactory, socketConfiguration,tokenSource);
        }


    }
}