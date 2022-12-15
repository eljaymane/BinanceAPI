using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Infrastructure.Extensions;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Converters
{
    public class BinanceEventTypeConverter : JsonConverter<BinanceEventType>
    {
        private readonly Type[] _types;

        public BinanceEventTypeConverter()
        {

        }
        public BinanceEventTypeConverter(params Type[] types)
        {
            _types = types;
        }

        public override void WriteJson(JsonWriter writer, BinanceEventType value, JsonSerializer serializer)
        {

            writer.WriteRawValue($"\"e\":\"{value.GetStringValue()}\"");
        }

        public override BinanceEventType ReadJson(JsonReader reader, Type objectType, BinanceEventType existingValue, bool b, JsonSerializer serializer)
        {
            foreach (var param in Enum.GetNames(typeof(BinanceEventType)))
            {
                var kline = Enum.Parse<BinanceEventType>(param);
                var str = kline.GetStringValue();
                if (str == reader?.Value!.ToString()) return Enum.Parse<BinanceEventType>(param);
            }
            return BinanceEventType.Kline;
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

