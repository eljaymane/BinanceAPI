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
    public class BinanceStreamResponse<T> where T : IBinanceStreamData 
    {
        [JsonProperty("e")]
        public BinanceEventType EventType { get; set; }
        [JsonProperty("E"), JsonConverter(typeof(UnixTimestampDateConverter))]
        public DateTime TimeStamp { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("lastUpdateId")]
        public long? lastUpdateId { get; set; }
        [JsonProperty("k")]
        public T? Data { get; set; }
    }
}
