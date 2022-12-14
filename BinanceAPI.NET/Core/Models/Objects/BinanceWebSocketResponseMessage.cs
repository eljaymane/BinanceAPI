using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models.Enums;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Models.Objects
{
    [Serializable]
    public class BinanceWebSocketResponseMessage<T> where T : IBinanceStreamData
    {
        [JsonProperty("e")]
        public BinanceEventType EventType { get; set; }
        [JsonProperty("E"), JsonConverter(typeof(UnixTimestampDateConverter))]
        public DateTime EventTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("k")]
        public T Data { get; set; }

        public BinanceWebSocketResponseMessage()
        {

        }

        public BinanceWebSocketResponseMessage(BinanceEventType eventType, DateTime eventTime, string symbol, T data)
        {
            EventType = eventType;
            EventTime = eventTime;
            Symbol = symbol;
            Data = data;
        }


    }
    [Serializable]
    public class BinanceWebSocketResponseMessage : IBinanceResponse
    {
        [JsonProperty("e")]
        public BinanceEventType EventType { get; set; }
        [JsonProperty("E"), JsonConverter(typeof(UnixTimestampDateConverter))]
        public DateTime EventTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("k")]
        public IBinanceStreamData Data { get; set; }

        public BinanceWebSocketResponseMessage()
        {

        }

        public BinanceWebSocketResponseMessage(BinanceEventType eventType, DateTime eventTime, string symbol, IBinanceStreamData data)
        {
            EventType = eventType;
            EventTime = eventTime;
            Symbol = symbol;
            Data = data;
        }


    }
}