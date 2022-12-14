using BinanceAPI.NET.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BinanceAPI.NET.Core.Models.Objects.StreamData
{
    public class BinanceRollingWindowStatsData : IBinanceResponse
    {
        [JsonProperty("p")]
        public decimal PriceChange { get; set; }
        [JsonProperty("P")]
        public decimal PriceChangePerCent { get; set; }
        [JsonProperty("o")]
        public decimal OpenPrice { get; set; }
        [JsonProperty("h")]
        public decimal HighPrice { get; set; }
        [JsonProperty("l")]
        public decimal LowPrice { get; set; }
        [JsonProperty("c")]
        public decimal LastPrice { get; set; }
        [JsonProperty("w")]
        public decimal WeightedAvgPrice { get; set; }
        [JsonProperty("v")]
        public decimal TotalTradedBaseAssetVol { get; set; }
        [JsonProperty("q")]
        public decimal TotalTradedQuoteAssetVol { get; set; }
        [JsonProperty("O"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime StatsOpenTime { get; set; }
        [JsonProperty("C"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime StatsCloseTime { get; set; }
        [JsonProperty("F")]
        public long FirstTradeId { get; set; }
        [JsonProperty("L")]
        public long LastTradeId { get; set; }
        [JsonProperty("n")]
        public long TotalNumberOfTrades { get; set; }
    }
}