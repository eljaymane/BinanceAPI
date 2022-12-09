using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Socket;
using BinanceAPI.NET.Infrastructure.Abstractions;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using BinanceBOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceIndividualSymbolTickerStream : AbstractBinanceStream
    {
        public BinanceIndividualSymbolTickerStream(string symbol, CancellationTokenSource tokenSource) : base(symbol,BinanceStreamType.IndividualSymbolTicker, tokenSource)
        {
        }
    }
}
