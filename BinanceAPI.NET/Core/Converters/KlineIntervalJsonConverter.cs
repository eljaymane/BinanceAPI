using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Infrastructure.Extensions;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Converters
{
    public class KlineIntervalJsonConverter : JsonConverter<KlineInterval>
    {
        private readonly Type[] _types;

        public KlineIntervalJsonConverter(params Type[] types)
        {
            _types = types;
        }

        public override void WriteJson(JsonWriter writer, KlineInterval value, JsonSerializer serializer)
        {
           
            writer.WriteRawValue($"\"interval\":\"{value.GetStringValue()}\"");
        }

        public override KlineInterval ReadJson(JsonReader reader, Type objectType, KlineInterval existingValue,bool b, JsonSerializer serializer)
        {
            foreach (var param in Enum.GetNames(typeof(KlineInterval)))
            {
                var kline = Enum.Parse<KlineInterval>(param);
                var str = kline.GetStringValue();
                if (str == reader.Value.ToString()) return Enum.Parse<KlineInterval>(param);
            }
            return KlineInterval.OneMinute;
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
