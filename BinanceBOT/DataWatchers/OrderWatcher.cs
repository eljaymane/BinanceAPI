using Binance.Net.Interfaces;
using BinanceBOT.Core;
using BinanceBOT.Core.Model.Abstractions;
using BinanceBOT.Core.Model.Market;
using CryptoExchange.Net.CommonObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceBOT.DataWatchers
{
    internal class OrderWatcher : BaseWatcher<IBinanceTick>
    {
        public OrderWatcher(AbstractContext marketContext) : base(marketContext)
        {
        }

        public override IBinanceTick Handle(IBinanceTick Data)
        {
            //logger.LogDebug(Ticker.BestAskPrice + " || " + Ticker.BestAskQuantity);

            //var context = (MarketContext)MarketContext;
            //context.UpdateUserAssets().Wait();
            //var userAssetBase = context.userBalances.GetValueOrDefault(context.BaseSymbol);
            //var userTargetAsset = context.GetUserAsset(context.TargetSymbol).Result;
            //var maxBuyQuantity = Pricing.MaxBuyQuantity(Ticker.BestAskPrice, userTargetAsset.Available);
            //if (context != null)
            //{
            //    if (userAssetBase != null)
            //    {
            //        if (Ticker.BestAskPrice >= context._MarketData.GetBestSellPrice() && context.placedOrders.Where(e => e.Value.Symbol.ToLower() == tick.Symbol.ToLower()).Count() == 0)
            //        {
            //            if (Pricing.DoubleToDecimal(userAssetBase.Available) != 0) context.PlaceSellOrderAsync(Pricing.DoubleToDecimal(userAssetBase.Available), Ticker.LastPrice, CommonOrderSide.Sell, CommonOrderType.Limit, null);
            //        }

            //    }
            //    if (userTargetAsset != null)
            //    {
            //        if (Ticker.BestBidPrice <= context._MarketData.GetBestBuyPrice() && context.placedOrders.Where(e => e.Value.Symbol.ToLower() == tick.Symbol.ToLower()).Count() == 0)
            //        {
            //            if (maxBuyQuantity != 0 && userTargetAsset.Available >= Pricing.GetPrice(maxBuyQuantity, Pricing.DoubleToDecimal(context._MarketData.GetBestBuyPrice()))) context.PlaceOrderBuyAsync(maxBuyQuantity, Ticker.BestAskPrice, null);
            //        }
            //    }
            //}
            //Thread.Sleep(1000);
            //return Ticker;
            return Data;
        }
    }
}
