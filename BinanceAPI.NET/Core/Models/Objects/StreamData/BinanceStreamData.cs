using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models.Enums;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Models.Objects.StreamData
{
    [Serializable]
    public class BinanceStreamData : IBinanceStreamData
    {
        [JsonProperty("e")]
        public BinanceEventType EventType { get; set; }
        [JsonProperty("E"), JsonConverter(typeof(UnixTimestampDateConverter))]
        public DateTime EventTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }


        public BinanceStreamData()
        {

        }

        public BinanceStreamData(BinanceEventType eventType, DateTime eventTime, string symbol)
        {
            EventType = eventType;
            EventTime = eventTime;
            Symbol = symbol;
        }


    }
    [Serializable]
    public class BinanceStreamData<T> : IBinanceStreamData
    {
        [JsonProperty("e")]
        public BinanceEventType EventType { get; set; }
        [JsonProperty("E"), JsonConverter(typeof(UnixTimestampDateConverter))]
        public DateTime EventTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("k")]
        public T Data { get; set; }

        public BinanceStreamData()
        {

        }
    }
}