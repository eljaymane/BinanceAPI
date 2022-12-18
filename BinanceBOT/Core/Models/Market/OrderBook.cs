using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinanceBOT.Core.Model.Abstractions;

namespace BinanceBOT.Core.Model.Market
{
    public class OrderBook : AbstractEntity
    {
        public long lastUpdateId;
        public List<string[]> asks { get; set; }
        public List<string[]> bids { get; set; }

        public OrderBook()
        {
            asks = new();
            bids = new();
        }
    }
}
