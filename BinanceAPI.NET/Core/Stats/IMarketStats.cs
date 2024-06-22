using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects.Entities;
using BinanceAPI.NET.Core.Stats.Historical.Klines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Stats
{
    public interface IMarketStats
    {
        Task CalculateSMA(IEnumerable<BinanceKlineHistoricalData> entries);
        Task CalculateEMA(IEnumerable<BinanceKlineHistoricalData> entries);

    }
}
