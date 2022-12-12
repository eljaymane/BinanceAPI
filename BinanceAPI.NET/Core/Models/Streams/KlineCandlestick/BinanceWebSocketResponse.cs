using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Models.Streams.KlineCandlestick
{
    [Serializable]
    public class BinanceWebSocketResponse<T> : IResponseDataType
    {
        [JsonProperty("e")]
        public BinanceEventType EventType { get; set; }
        [JsonProperty("E")]
        public long EventTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("k")]
        public T Data { get; set; }

        public BinanceWebSocketResponse()
        {

        }

        public BinanceWebSocketResponse(BinanceEventType eventType, long eventTime, string symbol,T data)
        {
            EventType = eventType;
            EventTime = eventTime;
            Symbol = symbol;
            Data = data;
        }
    }
}