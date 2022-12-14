using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Objects.StreamData
{
    [Serializable]
    public class BinanceKlineCandlestickData : IBinanceResponse, IBinanceStreamData
    {
        [JsonProperty("t"), JsonConverter(typeof(UnixTimestampDateConverter))]
        public DateTime StartTime { get; set; }
        [JsonProperty("T"), JsonConverter(typeof(UnixTimestampDateConverter))]
        public DateTime EndTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("i")]
        public KlineInterval Interval { get; set; }
        [JsonProperty("f")]
        public long FirstTradeId { get; set; }
        [JsonProperty("L")]
        public long LastTradeId { get; set; }
        [JsonProperty("o")]
        public double OpenPrice { get; set; }
        [JsonProperty("c")]
        public double ClosePrice { get; set; }
        [JsonProperty("h")]
        public double HighPrice { get; set; }
        [JsonProperty("l")]
        public double LowPrice { get; set; }
        [JsonProperty("v")]
        public double BaseAssetVolume { get; set; }
        [JsonProperty("n")]
        public int NumberOfTrades { get; set; }
        [JsonProperty("x")]
        public bool IsClosed { get; set; }
        [JsonProperty("q")]
        public double QuoteAssetVolume { get; set; }
        [JsonProperty("V")]
        public double TakerBuyBaseVolume { get; set; }
        [JsonProperty("Q")]
        public double TakerBuyQuoteVolume { get; set; }
        [JsonProperty("B")]
        public string ignore { get; set; }

        public BinanceKlineCandlestickData()
        {

        }

    }
}
