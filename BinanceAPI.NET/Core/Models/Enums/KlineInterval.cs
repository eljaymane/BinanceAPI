using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Infrastructure.Attributes;
using System.Text.Json.Serialization;

namespace BinanceAPI.NET.Core.Models.Enums
{
    [Serializable]
    [JsonConverter(typeof(KlineIntervalJsonConverter))]
    public enum KlineInterval
    {
        [StringValue("1s")]
        OneSecond,
        [StringValue("1m")]
        OneMinute,
        [StringValue("3m")]
        ThreeMinutes,
        [StringValue("5m")]
        FiveMinutes,
        [StringValue("15m")]
        FifteenMinutes,
        [StringValue("30m")]
        ThirtyMinutes,
        [StringValue("1h")]
        OneHour,
        [StringValue("2h")]
        TwoHours,
        [StringValue("4h")]
        FourHours,
        [StringValue("6h")]
        SixHours,
        [StringValue("12h")]
        TwelveHours,
        [StringValue("1d")]
        OneDay,
        [StringValue("3d")]
        ThreeDays,
        [StringValue("1w")]
        OneWeek,
        [StringValue("1M")]
        OneMonth

    }
}
