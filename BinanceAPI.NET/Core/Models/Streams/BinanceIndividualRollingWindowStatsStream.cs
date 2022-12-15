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
    public class BinanceIndividualRollingWindowStatsStream : AbstractBinanceStream<BinanceRollingWindowStatsData>
    {
        public BinanceIndividualRollingWindowStatsStream(ref BinanceMarketDataService client) : base(ref client, BinanceStreamType.IndividualRollingWindowStats)
        {
        }

        public void SubscribeAsync(string symbol, BinanceStatisticsRollingWindowSize? windowSize = BinanceStatisticsRollingWindowSize.OneHour)
        {
            var request = new BinanceWebSocketRequestMessage(0,
               BinanceRequestMessageType.Subscribe, new string[] { symbol.ToLower() +  StreamType.GetStringValue()! + windowSize!.GetStringValue() });
            //Client.SendRequestAsync(request);
        }
       
    }
}
