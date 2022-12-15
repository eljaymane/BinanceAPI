using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Interfaces
{
    public interface IBinanceStreamData 
    {
        public BinanceEventType EventType { get; set; }
        public DateTime EventTime { get; set; }
        public string Symbol { get; set; }
        public static JsonSerializerSettings GetSerializationSettings()
        {
            var serializationSettings = new JsonSerializerSettings
            {
                Converters = { new UnixTimestampDateConverter(), new KlineIntervalJsonConverter(), new BinanceEventTypeConverter(), new BinanceStatisticsRollingWindowSizeConverter() }
            };
            return serializationSettings;
        }
    }
}