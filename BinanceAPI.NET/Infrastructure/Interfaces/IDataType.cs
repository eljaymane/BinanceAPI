using BinanceAPI.NET.Core.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    public interface IRequestDataType
    {
        public static JsonSerializerSettings GetSerialiazationSettings()
        {
            var settings = new JsonSerializerSettings
            {
                Converters = { new RequestMessageTypeJsonConverter() }
            };
            return settings;
        }
    }
}
