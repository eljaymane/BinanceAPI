using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Spot.Socket;
using BinanceBOT.Core;
using BinanceBOT.Core.Model.Abstractions;
using BinanceBOT.Core.Model.Market;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace BinanceBOT.DataWatchers
{
    public class KlineDataWatcher : BaseWatcher<IBinanceStreamKlineData>
    {

        public KlineDataWatcher(ref AbstractContext context) : base(context)
        {
        }

        public override IBinanceStreamKlineData Handle(IBinanceStreamKlineData Tick )
        {
           ((MarketDataContext)MarketContext).UpdateKlineTicker(Tick);
            return Tick;

        }

        public void Update(DataEvent<IBinanceStreamKlineData> Data )
        {
            Handle(Data.Data);
        }
    }
}