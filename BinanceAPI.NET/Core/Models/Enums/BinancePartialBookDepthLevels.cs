using BinanceAPI.NET.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Enums
{
    [Serializable]
    public enum BinancePartialBookDepthLevels
    {
        [StringValue("5")]
        Five,
        [StringValue("10")]
        Ten,
        [StringValue("20")]
        Twenty
    }
}
