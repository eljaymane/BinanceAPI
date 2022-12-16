using BinanceAPI.NET.Core.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Objects.StreamData
{
    public class BinanceTradeData : BinanceStreamData
    {
        [JsonProperty("t")]
        public int TradeId { get; set; }
        [JsonProperty("p")]
        public decimal Price { get; set; }
        [JsonProperty("q")]
        public decimal Quantity { get; set; }
        [JsonProperty("b")]
        public int BuyerOrderId { get; set; }
        [JsonProperty("a")]
        public int SellerOrderId { get; set; }
        [JsonProperty("T"), JsonConverter(typeof(UnixTimestampDateConverter))]
        public DateTime TradeTime { get; set; }
        [JsonProperty("m")]
        public bool IsBuyerMarketMaker { get; set; }
        [JsonProperty("M")]
        public bool Ignore { get; set; }
    }
}
