using BinanceAPI.NET.Core.Converters;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Interfaces
{
    public interface IBinanceResponse<T> where T : IBinanceStreamData 
    {
        public T Data { get; set; }

    }
}