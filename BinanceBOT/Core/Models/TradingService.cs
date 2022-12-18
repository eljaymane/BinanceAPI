using BinanceBOT.Core.Model.Market;
using BinanceBOT.DataWatchers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BinanceBOT.Core.Model
{
    public class TradingService
    {
        private ILoggerFactory _loggerFactory;
        private GlobalConfig Config;
        private ILogger<TradingService> _logger;
        private TickerWatcherFactory watcherFactory;


        public TradingService(ILoggerFactory loggerFactory, GlobalConfig config,TickerWatcherFactory factory)
        {
            _loggerFactory = loggerFactory;
            Config = config;
            watcherFactory= factory;
        }

        public void Initialize(string BaseSymbol, string TargetSymbol)
        {
            
        }

    }
}
