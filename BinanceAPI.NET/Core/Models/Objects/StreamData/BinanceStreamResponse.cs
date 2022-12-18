using BinanceAPI.NET.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Objects.StreamData
{
    public class BinanceStreamResponse<T> where T : IBinanceStreamData 
    {
        [JsonProperty("stream")]
        public string Stream { get; set; }
        [JsonProperty("data")]
        public T? Data { get; set; }
    }
}
