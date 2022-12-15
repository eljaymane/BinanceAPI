using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceTradeStream : AbstractBinanceStream<BinanceTradeData>
    {
        public BinanceTradeStream(BinanceMarketDataService client) : base(ref client, BinanceStreamType.Trade)
        {
        }

        public void SubscribeAsync(string symbol)
        {
            Client.SubscribeAsync(symbol + StreamType.GetStringValue());
        }
    }
}
