using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BinanceAPI.NET.Core.Models.Objects.StreamData
{
    [Serializable]
    public class BinanceTickerData : IResponseDataType, IBinanceStreamData
    {
        [JsonProperty("p")]
        public decimal PriceChange { get; set; }
        [JsonProperty("P")]
        public decimal PriceChangePerCent { get; set; }
        [JsonProperty("w")]
        public decimal WeightedAvgPrice { get; set; }
        [JsonProperty("x")]
        public decimal FirstTrade { get; set; }
        [JsonProperty("c")]
        public decimal LastPrice { get; set; }
        [JsonProperty("Q")]
        public decimal LastQuantity { get; set; }
        [JsonProperty("b")]
        public decimal BestBidPrice { get; set; }
        [JsonProperty("B")]
        public decimal BestBidQuantity { get; set; }
        [JsonProperty("a")]
        public decimal BestAskPrice { get; set; }
        [JsonProperty("A")]
        public decimal BestAskQuantity { get; set; }
        [JsonProperty("o")]
        public decimal OpenPrice { get; set; }
        [JsonProperty("h")]
        public decimal HighPrice { get; set; }
        [JsonProperty("l")]
        public decimal LowPrice { get; set; }
        [JsonProperty("v")]
        public decimal TotalTradedBaseAsset { get; set; }
        [JsonProperty("q")]
        public decimal TotalTradedQuoteAsset { get; set; }
        [JsonProperty("O"), JsonConverter((typeof(UnixDateTimeConverter)))]
        public DateTime OpenTime { get; set; }
        [JsonProperty("C"), JsonConverter((typeof(UnixDateTimeConverter)))]
        public DateTime CloseTime { get; set; }
        [JsonProperty("F")]
        public long FirstTradeId { get; set; }
        [JsonProperty("L")]
        public long LastTradeId { get; set; }
        [JsonProperty("n")]
        public long TotalNumberOfTrades { get; set; }
    }
}