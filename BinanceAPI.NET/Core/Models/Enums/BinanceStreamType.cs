using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Infrastructure.Attributes;
using System.Text.Json.Serialization;

namespace BinanceAPI.NET.Core.Models.Enums
{
    [JsonConverter(typeof(StreamTypeJsonConverter))]
    public enum BinanceStreamType
    {
        [StringValue("@kline_")]
        KlineCandlestick,
        [StringValue("@aggrTrade")]
        AgregateTrades,
        [StringValue("@markPrice")]
        MarkPrice,
        [StringValue("@miniTicker")]
        IndividualSymbolMiniTicker,
        [StringValue("@ticker")]
        IndividualSymbolTicker,
        [StringValue("@depth")]
        PartialBookDepth

    }
}