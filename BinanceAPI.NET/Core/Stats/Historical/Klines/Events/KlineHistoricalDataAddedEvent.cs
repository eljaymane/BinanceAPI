using PEzbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Stats.Historical.Klines.events
{
    public sealed class KlineHistoricalDataAddedEvent : IEzEvent
    {
        public string Symbol { get; }
        public IEnumerable<BinanceKlineHistoricalData> Data { get; }

        public KlineHistoricalDataAddedEvent(string symbol, IEnumerable<BinanceKlineHistoricalData> data)
        {
            Symbol = symbol;
            Data = data;
        }
    }
}
