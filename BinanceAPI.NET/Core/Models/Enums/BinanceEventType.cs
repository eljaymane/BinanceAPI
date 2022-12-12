using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Infrastructure.Attributes;
using System.Text.Json.Serialization;

namespace BinanceAPI.NET.Core.Models.Enums
{
    [Serializable]
    [JsonConverter(typeof(BinanceEventTypeConverter))]
    public enum BinanceEventType
    {
        [StringValue("kline")]
        Kline,
        [StringValue("value")]
        value
    }
}
