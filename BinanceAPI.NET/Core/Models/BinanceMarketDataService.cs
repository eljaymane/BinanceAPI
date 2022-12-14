using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Streams;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace BinanceAPI.NET.Core.Models
{
    public class BinanceMarketDataService
    {
        public ILoggerFactory LoggerFactory { get; set; }
        public BinanceKlineCandlestickStream? KlineStream { get; set; }
        public BinanceIndividualSymbolTickerStream TickerStream { get; set; }
        private BinanceAllMarketTickerStream AllMarketTickerStream { get; set; }
        private BinanceIndividualBookTicker IndividualBookTickerStream { get; set; }
        public BinanceIndividualMiniTickerStream IndividualMiniTickerStream { get; set; }
        public BinanceIndividualRollingWindowStatsStream RollingWindowStatsStream { get; set; }
        public BinancePartialBookDepthStream PartialBookDepthStream { get; set; }
        public BinanceAllMarketMiniTickerStream AllMarketMiniTickerStream { get; set; }
        public BinanceAllMarketRollingWindowStatsStream AllMarketRollingWindowStatsStream { get; set; }

        public CancellationTokenSource TokenSource { get; set; }

        public BinanceMarketDataService(ILoggerFactory loggerFactory,SocketConfiguration socketConfiguration, CancellationTokenSource tokenSource)
        {
            LoggerFactory= loggerFactory;
            TokenSource = tokenSource;
            KlineStream = new (loggerFactory, socketConfiguration,tokenSource);
            TickerStream = new (socketConfiguration,loggerFactory,tokenSource);
            AllMarketTickerStream = new (socketConfiguration,loggerFactory,tokenSource);
            IndividualMiniTickerStream = new(socketConfiguration,loggerFactory,tokenSource);
            IndividualBookTickerStream = new(socketConfiguration, loggerFactory, tokenSource);
            RollingWindowStatsStream = new(socketConfiguration,loggerFactory,tokenSource);
            PartialBookDepthStream = new(socketConfiguration,loggerFactory,tokenSource);
            AllMarketMiniTickerStream = new(socketConfiguration,loggerFactory,tokenSource);
            AllMarketRollingWindowStatsStream = new(socketConfiguration,loggerFactory,tokenSource);
        }


    }
}