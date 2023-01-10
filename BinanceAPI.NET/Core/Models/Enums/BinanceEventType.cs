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
        [StringValue("24hrTicker")]
        TwentyFourHourTicker,
        [StringValue("4hTicker")]
        FourHoursRollingWindow,
        [StringValue("1hTicker")]
        OneHourRollingWindow,
        [StringValue("1dTicker")]
        OneDayRollingWindow
    }
}
