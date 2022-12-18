using Binance.Common;
using BinanceAPI.NET.Core.Models.Socket.Clients;
using BinanceAPI.NET.Infrastructure.Interfaces;
using BinanceBOT.Core.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceBOT.Core.Models.Abstractions
{
    public class MarketDataContext : AbstractContext
    {
        private BinanceMarketDataClient Client;
        public MarketDataContext(string baseSymbol, string targetSymbol) : base(baseSymbol, targetSymbol)
        {
        
        }
    }
}
