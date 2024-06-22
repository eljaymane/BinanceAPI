using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Infrastructure.Extensions;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;

namespace BinanceAPI.NET.Core.Converters
{
    public class BinancePartialBookDepthLevelsConverter : JsonConverter<BinancePartialBookDepthLevels>
    {
        public override BinancePartialBookDepthLevels ReadJson(JsonReader reader, Type objectType, BinancePartialBookDepthLevels existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            foreach (var param in Enum.GetNames(typeof(BinancePartialBookDepthLevels)))
            {
                var kline = Enum.Parse<BinancePartialBookDepthLevels>(param);
                var str = kline.GetStringValue();
                if (str == reader.Value.ToString()) return Enum.Parse<BinancePartialBookDepthLevels>(param);
            }
            return BinancePartialBookDepthLevels.Ten;
        }

        public override void WriteJson(JsonWriter writer, BinancePartialBookDepthLevels value, JsonSerializer serializer)
        {
            writer.WriteRawValue($"\"depth\":\"{value.GetStringValue()}\"");
        }
    }
}
