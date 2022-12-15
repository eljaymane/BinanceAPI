using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
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
        public BinancePartialBookDepthStream(ref BinanceMarketDataService client) : base(ref client, BinanceStreamType.PartialBookDepth)
        {
        }
       
    }
}
