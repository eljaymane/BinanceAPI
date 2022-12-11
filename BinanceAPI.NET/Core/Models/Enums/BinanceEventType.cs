using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Infrastructure.Attributes;
using System.Text.Json.Serialization;

namespace BinanceAPI.NET.Core.Models.Enums
{
    [JsonConverter(typeof(BinanceEventTypeConverter))]
    public enum BinanceEventType
    {
        [StringValue("kline")]
        Kline
    }
}
