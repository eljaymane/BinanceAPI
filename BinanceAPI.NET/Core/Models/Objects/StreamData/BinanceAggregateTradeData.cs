using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceAggregateTradeData : BinanceStreamData
    {
        [JsonProperty("a")]
        public int AggTradeId { get; set; }
        [JsonProperty("p")]
        public decimal Price { get; set; }
        [JsonProperty("q")]
        public decimal Quantity { get; set; }
        [JsonProperty("f")]
        public int FirstTradeId { get; set; }
        [JsonProperty("l")]
        public int LastTradeId { get; set; }
        [JsonProperty("T"),JsonConverter(typeof(UnixTimestampDateConverter))]
        public DateTime TradeTime { get; set; }
        [JsonProperty("m")]
        public bool IsBuyerMarketMaker { get; set; }
        [JsonProperty("M")]
        public bool Ignore { get; set; }
    }
}