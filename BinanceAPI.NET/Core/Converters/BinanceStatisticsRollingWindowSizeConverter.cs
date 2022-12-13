using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Infrastructure.Extensions;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Converters
{
    public class BinanceStatisticsRollingWindowSizeConverter : JsonConverter<BinanceStatisticsRollingWindowSize>
    {
        private readonly Type[] _types;

        public BinanceStatisticsRollingWindowSizeConverter(params Type[] types)
        {
            _types = types;
        }

        public override void WriteJson(JsonWriter writer, BinanceStatisticsRollingWindowSize value, JsonSerializer serializer)
        {

            writer.WriteRawValue($"\"windowSize\":\"{value.GetStringValue()}\"");
        }

        public override BinanceStatisticsRollingWindowSize ReadJson(JsonReader reader, Type objectType, BinanceStatisticsRollingWindowSize existingValue, bool b, JsonSerializer serializer)
        {
            foreach (var param in Enum.GetNames(typeof(BinanceStatisticsRollingWindowSize)))
            {
                var kline = Enum.Parse<BinanceStatisticsRollingWindowSize>(param);
                var str = kline.GetStringValue();
                if (str == reader.Value.ToString()) return Enum.Parse<BinanceStatisticsRollingWindowSize>(param);
            }
            return BinanceStatisticsRollingWindowSize.OneDay;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public bool CanConvert(Type objectType)
        {
            return _types.Any(t => t == objectType);
        }
    }
}