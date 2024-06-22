using PEzbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Stats.Historical.Klines.Events
{
    public class KlineDataReceivedEvent : IEzEvent
    {
        public string Symbol { get; set; }
        public BinanceKlineHistoricalData KlineData { get; set; }

        public KlineDataReceivedEvent(string symbol, BinanceKlineHistoricalData klineData)
        {
            Symbol = symbol;
            KlineData = klineData;
        }
    }
}
