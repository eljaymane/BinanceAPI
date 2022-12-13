using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Infrastructure.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Enums
{
    [JsonConverter(typeof(BinanceStatisticsRollingWindowSizeConverter))]
    public enum BinanceStatisticsRollingWindowSize
    {
        [StringValue("1h")]
        OneHour,
        [StringValue("4h")]
        FourHours,
        [StringValue("1d")]
        OneDay
    }
}
