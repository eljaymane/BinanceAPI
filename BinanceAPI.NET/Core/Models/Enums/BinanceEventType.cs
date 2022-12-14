using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Infrastructure.Attributes;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Models.Enums
{
    [Serializable]
    [JsonConverter(typeof(BinanceEventTypeConverter))]
    public enum BinanceEventType
    {
        [StringValue("kline")]
        Kline,
        [StringValue("24hrMiniTicker")]
        TwentyFourHoursMiniTicker,
        [StringValue("1hTicker")]
        OneHourTicker
    }
}
