using BinanceAPI.NET.Core.Models.Enums;
using System.Text.Json.Serialization;
using System.Text.Json;
using BinanceAPI.NET.Infrastructure.Extensions;

namespace BinanceAPI.NET.Core.Converters
{
    public class BinanceEventTypeConverter : JsonConverter<BinanceEventType>
    {
            public override BinanceEventType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<BinanceEventType>(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, BinanceEventType value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.GetStringValue());
            }
    }

}

