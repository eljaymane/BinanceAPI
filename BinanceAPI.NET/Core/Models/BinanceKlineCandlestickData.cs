using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models
{
    [Serializable]
    public class BinanceKlineCandlestickData : IBinanceStreamData
    {
        [JsonPropertyName("t")]
        public TimeSpan StartTime { get; set; }
        [JsonPropertyName("T")]
        public TimeSpan EndTime { get; set; }
        [JsonPropertyName("s")]
        public string Symbol { get; set; }
        [JsonPropertyName("i"), JsonConverter(typeof(KlineIntervalJsonConverter))]
        public KlineInterval Interval { get; set; }
        [JsonPropertyName("f")]
        public int FirstTradeId { get; set; }
        [JsonPropertyName("L")]
        public int LastTradeId { get; set; }
        [JsonPropertyName("o")]
        public decimal OpenPrice { get; set; }
        [JsonPropertyName("c")]
        public decimal ClosePrice { get; set; }
        [JsonPropertyName("h")]
        public decimal HighPrice { get; set; }
        [JsonPropertyName("l")]
        public decimal LowPrice { get; set; }
        [JsonPropertyName("v")]
        public decimal BaseAssetVolume { get; set; }
        [JsonPropertyName("n")]
        public int NumberOfTrades { get; set; }
        [JsonPropertyName("x")]
        public bool IsClosed { get; set; }
        [JsonPropertyName("q")]
        public decimal QuoteAssetVolume { get; set; }
        [JsonPropertyName("V")]
        public decimal TakerBuyBaseVolume { get; set; }
        [JsonPropertyName("Q")]
        public decimal TakerBuyQuoteVolume { get; set; }


    }
}
