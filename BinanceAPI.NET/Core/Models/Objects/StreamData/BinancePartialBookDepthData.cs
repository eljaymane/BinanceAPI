using BinanceAPI.NET.Core.Interfaces;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Models.Objects.StreamData
{
    public class BinancePartialBookDepthData : IBinanceStreamData
    {
        [JsonProperty("lastUpdateId")]
        public long lastUpdateId { get; set; }
        [JsonProperty("bids")]
        public decimal[][] bids {get;set;}
        [JsonProperty("asks")]
        public decimal[][] asks {get;set;}
    }
}