using BinanceAPI.NET.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Objects.StreamData
{
    [Serializable]
    public class BinanceMiniTickerData : IBinanceStreamData
    {
        [JsonProperty("c")]
        public decimal ClosePrice { get; set; }
        [JsonProperty("o")]
        public decimal OpenPrice { get; set; }
        [JsonProperty("h")]
        public decimal HighPrice { get; set; }
        [JsonProperty("l")]
        public decimal LowPrice { get; set; }
        [JsonProperty("v")]
        public decimal TotalTradedBaseAssetVolume { get; set; }
        [JsonProperty("q")]
        public decimal TotalTradedQuoteAssetVolume { get; set; }
    }
}
