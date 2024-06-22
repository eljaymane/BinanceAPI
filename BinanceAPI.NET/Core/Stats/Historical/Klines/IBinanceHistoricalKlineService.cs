using BinanceAPI.NET.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Stats.Historical.Klines
{
    public interface IBinanceHistoricalKlineService
    {
        void DonwloadAndProcess(string symbol, string year, string month, KlineInterval interval);
        IEnumerable<IBinanceHistoricalData> GetMostRecentData(string symbol,int periods);
    }
}
