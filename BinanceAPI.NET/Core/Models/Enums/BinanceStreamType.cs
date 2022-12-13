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
        [StringValue("!miniTicker@arr")]
        AllMarketMiniTicker,
        [StringValue("!ticker@arr")]
        AllMarketTicker,
        [StringValue("@ticker_")]
        IndividualRollingWindowStats,
        [StringValue("!ticker_<param>@arr")]
        AllMarketRollingWindowStats,
        [StringValue("@bookTicker")]
        IndividualBookTicker,
        [StringValue("@depth")]
        PartialBookDepth

    }
}