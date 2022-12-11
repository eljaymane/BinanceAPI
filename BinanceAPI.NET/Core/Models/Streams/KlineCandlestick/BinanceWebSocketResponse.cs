using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using System.Text.Json.Serialization;

namespace BinanceAPI.NET.Core.Models.Streams.KlineCandlestick
{
    [Serializable]
    public class BinanceWebSocketResponse
    {
        [JsonPropertyName("e"),JsonConverter(typeof(BinanceEventTypeConverter))]
        public BinanceEventType EventType { get; set; }
        [JsonPropertyName("E")]
        public TimeSpan EventTime { get; set; }
        [JsonPropertyName("s")]
        public string Symbol { get; set; }

        public dynamic Data { get; private set; }

        public BinanceWebSocketResponse(BinanceEventType eventType, TimeSpan eventTime, string symbol, dynamic data)
        {
            EventType = eventType;
            EventTime = eventTime;
            Symbol = symbol;
            Data = data;
        }
    }
}