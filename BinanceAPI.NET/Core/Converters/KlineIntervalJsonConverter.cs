using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Converters
{
    public class KlineIntervalJsonConverter : JsonConverter<KlineInterval>
    {
        public override KlineInterval Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<KlineInterval>(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, KlineInterval value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.GetStringValue());
        }
    }
}
