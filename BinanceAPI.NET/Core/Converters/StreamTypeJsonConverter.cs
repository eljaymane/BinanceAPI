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
    internal class StreamTypeJsonConverter : JsonConverter<BinanceStreamType>
    {
        public override BinanceStreamType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<BinanceStreamType>(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, BinanceStreamType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.GetStringValue());
        }
    }
}
