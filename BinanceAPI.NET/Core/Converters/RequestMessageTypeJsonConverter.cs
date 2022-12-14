using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Infrastructure.Extensions;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Converters
{
    internal class RequestMessageTypeJsonConverter : JsonConverter<BinanceRequestMessageType>
    {
        //public override bool CanConvert(Type typeToConvert)
        //{
        //    if (typeToConvert == typeof(BinanceRequestMessageType)) return true;
        //    return false;
        //}

        //public override BinanceRequestMessageType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //{
        //    return JsonSerializer.Deserialize<BinanceRequestMessageType>(reader.GetString());
        //}

        //public override void Write(Utf8JsonWriter writer, BinanceRequestMessageType value, JsonSerializerOptions options)
        //{
        //    writer.WriteStringValue(value.GetStringValue());
        //}
        public override BinanceRequestMessageType ReadJson(JsonReader reader, Type objectType, BinanceRequestMessageType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, BinanceRequestMessageType value, JsonSerializer serializer)
        {
            writer.WriteRawValue(value.GetStringValue());
        }
    }
}
