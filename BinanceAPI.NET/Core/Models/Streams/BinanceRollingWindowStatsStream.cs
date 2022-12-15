using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceRollingWindowStatsStream : AbstractBinanceStream<BinanceRollingWindowStatsData>
    {
        public BinanceRollingWindowStatsStream(BinanceMarketDataService client) : base(ref client, BinanceStreamType.IndividualRollingWindowStats)
        {
        }

        public void SubscribeAsync(string symbol, BinanceStatisticsRollingWindowSize? windowSize = BinanceStatisticsRollingWindowSize.OneHour)
        {
            Client.SubscribeAsync(symbol.ToLower() + StreamType.GetStringValue()! + windowSize!.GetStringValue());
        }
       
    }
}
