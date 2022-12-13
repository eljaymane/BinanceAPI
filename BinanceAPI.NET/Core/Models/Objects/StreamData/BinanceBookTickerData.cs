using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Models.Objects.StreamData
{
    [Serializable]
    public class BinanceBookTickerData
    {
        [JsonProperty("u")]
        public long OrderBookUpdateId { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("b")]
        public decimal BestBidPrice { get; set; }
        [JsonProperty("B")]
        public decimal BestBidQuantity { get; set; }
        [JsonProperty("a")]
        public decimal BestAskPrice { get; set; }
        [JsonProperty("A")]
        public decimal BestAskQuantity { get; set; }
    }
}