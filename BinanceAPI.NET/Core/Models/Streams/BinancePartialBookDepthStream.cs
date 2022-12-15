using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinancePartialBookDepthStream : AbstractBinanceStream<BinancePartialBookDepthData>
    {
        public BinancePartialBookDepthStream(BinanceMarketDataService client) : base(ref client, BinanceStreamType.PartialBookDepth)
        {
        }

        public void Subscribe(string symbol)
        {
            Client.SubscribeAsync(symbol.ToLower() + StreamType.GetStringValue());
        }

    }
}
