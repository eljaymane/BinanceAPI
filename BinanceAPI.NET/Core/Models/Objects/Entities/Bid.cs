using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Objects.Entities
{
    public class Bid
    {
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}
