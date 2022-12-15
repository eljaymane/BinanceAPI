using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;


namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceAllMarketRollingWindowStatsStream : AbstractBinanceStream<BinanceRollingWindowStatsData>
    {
        public BinanceAllMarketRollingWindowStatsStream(ref BinanceMarketDataService client) : base(ref client, BinanceStreamType.AllMarketRollingWindowStats)
        {
        }

        public void SubscribeAsync(BinanceStatisticsRollingWindowSize windowSize)
        {
            var request = new BinanceWebSocketRequestMessage(0,
              BinanceRequestMessageType.Subscribe, new string[] { StreamType.GetStringValue()!.Replace("<param>", windowSize.GetStringValue()) }) ;
           
            //Client.SendRequestAsync(request);
        }
       
    }
}
