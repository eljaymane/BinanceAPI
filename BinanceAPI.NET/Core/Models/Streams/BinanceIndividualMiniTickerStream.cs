using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceIndividualMiniTickerStream : AbstractBinanceStream<BinanceMiniTickerData>
    {
        public BinanceIndividualMiniTickerStream(ref BinanceMarketDataService client) : base(ref client, BinanceStreamType.IndividualSymbolTicker)
        {
                
        }
       
    }
}
