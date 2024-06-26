﻿using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceBookTickerStream : AbstractBinanceStream<BinanceBookTickerData>
    {
        public BinanceBookTickerStream(BinanceMarketDataService client) : base(ref client, BinanceStreamType.IndividualBookTicker)
        {
        }

        public void SubscribeAsync(string symbol)
        {
            Client.SubscribeAsync(symbol + StreamType.GetStringValue());
        }

        
    }
}
