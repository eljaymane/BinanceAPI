using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models.Objects.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BinanceAPI.NET.Core.Models.Objects.StreamData
{
    [Serializable]
    public class BinanceRollingWindowStatsData : BinanceRollingWindowStats
    {
        public BinanceRollingWindowStatsData()
        {

        }
    }
}