using BinanceAPI.NET.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Converters
{
    public class UnixTimestampDateConverter : JsonConverter<DateTime>
    {
        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return Calculation.EpochToDate((long)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
           writer.WriteRawValue(Calculation.DateToEpoch(value).ToString());
        }
    }
}
