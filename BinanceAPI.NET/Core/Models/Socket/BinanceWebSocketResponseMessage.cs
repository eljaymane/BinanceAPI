using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Models.Socket
{
    [Serializable]
    public class BinanceWebSocketResponseMessage<T> : IResponseDataType
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
}