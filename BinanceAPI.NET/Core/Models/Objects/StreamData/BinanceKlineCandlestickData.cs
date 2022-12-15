using BinanceAPI.NET.Core.Models.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Objects.StreamData
{
    [Serializable]
    public class BinanceKlineCandlestickData : BinanceStreamData<BinanceKlineCandlestick>
    {
    }
}
