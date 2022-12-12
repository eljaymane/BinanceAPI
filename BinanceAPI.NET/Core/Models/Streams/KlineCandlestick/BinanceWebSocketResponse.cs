using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Infrastructure.Interfaces;
using System.Text.Json.Serialization;

namespace BinanceAPI.NET.Core.Models.Streams.KlineCandlestick
{
    [Serializable]
    public class BinanceWebSocketResponse<T> : IResponseDataType
    {
        [JsonPropertyName("e"),JsonConverter(typeof(BinanceEventTypeConverter))]
        public BinanceEventType EventType { get; set; }
        [JsonPropertyName("E")]
        public TimeSpan EventTime { get; set; }
        [JsonPropertyName("s")]
        public string Symbol { get; set; }
        public T Data { get; set; }

        public BinanceWebSocketResponse(BinanceEventType eventType, TimeSpan eventTime, string symbol, T data)
        {
            EventType = eventType;
            EventTime = eventTime;
            Symbol = symbol;
            Data = data;
        }
    }
}