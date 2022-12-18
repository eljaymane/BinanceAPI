using Binance.Net.Interfaces;
using BinanceBOT.Core.Model.Abstractions;
using BinanceBOT.Core.Model.Market;
using BinanceBOT.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceBOT.DataWatchers
{
    public class TickerWatcherFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private GlobalConfig Config { get; set; }
        private static TickerWatcherFactory instance { get; set; }
        public Dictionary<string, TickerUpdatesWatcher> TickerWatchers = new Dictionary<string, TickerUpdatesWatcher>();
        public TickerWatcherFactory(ILoggerFactory loggerFactory, GlobalConfig config)
        {
            _loggerFactory = loggerFactory;
            instance = this;
            Config = config;
        }

        public static TickerWatcherFactory Instance { get { return instance; } }

        public TickerUpdatesWatcher Watch(AbstractContext marketContext,MarketDataContext marketDataContext, WalletContext ctx)
        {
            var watcher = new TickerUpdatesWatcher(marketContext.BaseSymbol+marketContext.TargetSymbol,ref marketContext,ref marketDataContext,ref ctx);
            watcher.Client = new SpotClient(_loggerFactory.CreateLogger<BaseClient>(), Config);
            watcher.logger = _loggerFactory.CreateLogger<TickerUpdatesWatcher>();
            return watcher;
        }

        public TickerUpdatesWatcher GetWatcher(string symbol)
        {
            return TickerWatchers.ContainsKey(symbol) ? TickerWatchers[symbol] : null;
        }

        public KlineDataWatcher WatchRollingWindow(AbstractContext context)
        {
            var watcher = new KlineDataWatcher(ref context);
            watcher.logger =_loggerFactory.CreateLogger<KlineDataWatcher>();
            return watcher;
        }

    }
}
