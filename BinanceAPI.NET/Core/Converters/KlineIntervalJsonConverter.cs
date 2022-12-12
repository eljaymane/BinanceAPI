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
            switch (reader.Value)
            {
                case "15m":
                    return KlineInterval.FifteenMinutes;

                default:
                    return KlineInterval.FiveMinutes;
            }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public bool CanConvert(Type objectType)
        {
            return _types.Any(t => t == objectType);
        }
        //public override bool CanConvert(Type typeToConvert)
        //{
        //    if (typeToConvert == typeof(KlineInterval)) return true;
        //    return false;

        //}
        //public override KlineInterval Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //{
        //    return JsonSerializer.Deserialize<KlineInterval>(reader.GetString());
        //}

        //public override void Write(Utf8JsonWriter writer, KlineInterval value, JsonSerializerOptions options)
        //{
        //    writer.WriteStringValue(value.GetStringValue());
        //}
    }
}
