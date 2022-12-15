using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Infrastructure.Abstractions;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceTickerStream : AbstractBinanceStream<BinanceTickerData>
    {
        public BinanceTickerStream(BinanceMarketDataService client) : base(ref client, BinanceStreamType.IndividualSymbolTicker)
        {
        }

        public void SubscribeAsync(string symbol)
        {  
            Client.SubscribeAsync(symbol.ToLower() + StreamType.GetStringValue());
        }

       
       
    }
}
