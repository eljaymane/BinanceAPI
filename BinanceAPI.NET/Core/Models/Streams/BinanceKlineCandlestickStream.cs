﻿using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects;
using BinanceAPI.NET.Core.Models.Objects.Entities;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;

namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceKlineCandlestickStream : AbstractBinanceStream<BinanceKlineCandlestickData>
    {
        public BinanceKlineCandlestickStream(BinanceMarketDataService client) : base(ref client,BinanceStreamType.KlineCandlestick)
        {
        }

        public Task SubscribeAsync(string symbol, KlineInterval interval = KlineInterval.OneHour)
        {
            Client.SubscribeAsync(symbol.ToLower() + StreamType.GetStringValue() + interval.GetStringValue());
            return Task.CompletedTask;
        }

    }
}
