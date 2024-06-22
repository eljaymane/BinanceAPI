using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Objects.Entities
{
    [Serializable]
    public class BinanceRollingWindowStats : IBinanceStreamData
    {
        [JsonProperty("e")]
        public BinanceEventType EventType { get; set; }
        [JsonProperty("E"), JsonConverter(typeof(UnixTimestampDateConverter))]
        public DateTime TimeStamp { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("p")]
        public double PriceChange { get; set; }
        [JsonProperty("P")]
        public double PriceChangePerCent { get; set; }
        [JsonProperty("o")]
        public double OpenPrice { get; set; }
        [JsonProperty("h")]
        public double HighPrice { get; set; }
        [JsonProperty("l")]
        public double LowPrice { get; set; }
        [JsonProperty("c")]
        public double LastPrice { get; set; }
        [JsonProperty("w")]
        public double WeightedAvgPrice { get; set; }
        [JsonProperty("v")]
        public double TotalTradedBaseAssetVol { get; set; }
        [JsonProperty("q")]
        public double TotalTradedQuoteAssetVol { get; set; }
        [JsonProperty("O"), JsonConverter(typeof(UnixTimestampDateConverter))]
        public DateTime StatsOpenTime { get; set; }
        [JsonProperty("C"), JsonConverter(typeof(UnixTimestampDateConverter))]
        public DateTime StatsCloseTime { get; set; }
        [JsonProperty("F")]
        public long FirstTradeId { get; set; }
        [JsonProperty("L")]
        public long LastTradeId { get; set; }
        [JsonProperty("n")]
        public long TotalNumberOfTrades { get; set; }
    }
}
