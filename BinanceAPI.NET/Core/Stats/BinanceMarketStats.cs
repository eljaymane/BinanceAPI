using BinanceAPI.NET.Core.Helpers;
using BinanceAPI.NET.Core.Models.Objects.Entities;
using BinanceAPI.NET.Core.Stats.Historical.Klines;
using BinanceAPI.NET.Core.Stats.Historical.Klines.events;
using PEzbus.CustomAttributes;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Stats
{
    public class BinanceMarketStats : IMarketStats
    {
        public static readonly int defaultEmaPeriods = 12;
        public static readonly int defaultMacdPeriods = 26;
        public string Symbol { get; set; }
        public sealed record SimpleMovingAverage(DateTime dateTime, decimal sma);
        public sealed record ExponentialMovingAverage(DateTime dateTime, decimal ema);
        public IList<SimpleMovingAverage> SmaList;
        public IList<ExponentialMovingAverage> EmaList;

        public BinanceMarketStats(string symbol)
        {
            SmaList = new List<SimpleMovingAverage>();
            EmaList = new List<ExponentialMovingAverage>();
            Symbol = symbol;
        }

        [Subscribe(typeof(KlineHistoricalDataAddedEvent))]
        public void HandleHistoricalDataAdded(KlineHistoricalDataAddedEvent e)
        {
            if (e.Symbol != Symbol) return;
            var data = e.Data.TakeLast(defaultEmaPeriods);
            CalculateSMA(data).Wait();
            CalculateEMA(data).Wait();
        }
        public Task CalculateSMA(IEnumerable<BinanceKlineHistoricalData> entries)
        {
            var periods = entries.Count();
            var sma = entries.Sum(entry => entry.Close);
            sma = sma / periods;
            var mostRecentDate = entries.OrderByDescending(entry => entry.OpenTime).First().OpenTime;

            SmaList.Add(new SimpleMovingAverage(mostRecentDate, sma));

            return Task.CompletedTask;
        }
        public Task CalculateEMA(IEnumerable<BinanceKlineHistoricalData> entries)
        {
            var periods = entries.Count();
            decimal ema= entries.Sum(x => x.Close)/entries.Count();

            foreach (var item in entries.TakeLast(periods - 1))
            {
                ema = EMA(ema, item.Close, periods);
            }
            var lastDateTime = entries.OrderByDescending(x => x.OpenTime).First().OpenTime;

            EmaList.Add(new ExponentialMovingAverage(lastDateTime, ema));

            return Task.CompletedTask;
        }

        private decimal EMA(decimal previousEMA,decimal closePrice,int periods)
        {
            decimal smoothingFactor = 2 / (periods + 1);
            return (decimal) (closePrice * smoothingFactor) + (previousEMA*(1-smoothingFactor)) ;
        }

        public string GetStats()
        {
            return $"SMA : {SmaList.Last().ToString()} - EMA : {EmaList.Last().ToString()}";
        }
    }
}
