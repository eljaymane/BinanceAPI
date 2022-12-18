using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Spot;
using BinanceBOT.Core;
using BinanceBOT.Core.Model.Abstractions;
using BinanceBOT.Core.Model.Market;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BinanceBOT.DataWatchers
{
    public class TickerUpdatesWatcher : BaseWatcher<IBinanceTick>
    {
        public IBinanceTick Ticker { get; set; }
        public string Symbol { get; set; }

        private event EventHandler TickerUpdated;

        private MarketDataContext marketDataContext;
        private WalletContext walletContext;
        public TickerUpdatesWatcher(string symbol,ref AbstractContext context,ref MarketDataContext marketDataCtx, ref WalletContext walletCtx) : base(context)
        {
            Symbol = symbol;
            marketDataContext= marketDataCtx;
            walletContext = walletCtx;  
        }

        public void SetContext(ref AbstractContext ctx)
        {
            MarketContext = ctx;
        }

        public void UpdateTicker(DataEvent<IBinanceTick> tick)
        {
            Ticker = tick.Data;
            marketDataContext.UpdateTicker(tick.Data);
            OnTickerUpdated(Ticker);
        }

        private protected void OnTickerUpdated(IBinanceTick tick)
        {
            TickerUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void Handle()
        {
            if(walletContext.userBalances.Count != 0)
            {
                var context = (TradingContext)MarketContext;
                var userAssetBase = walletContext.userBalances.GetValueOrDefault(context.BaseSymbol);
                var userTargetAsset = walletContext.userBalances.GetValueOrDefault(context.TargetSymbol);

                if (marketDataContext.GetBestSellPrice() != 0)
                {
                    if (userAssetBase != null)
                    {
                        var maxBuyQuantity = Pricing.MaxBuyQuantity(marketDataContext.GetBinanceTicker().LastPrice, userTargetAsset.Available);
                        if (marketDataContext.GetRTDTicker().LastPrice.Value >= marketDataContext.GetBestSellPrice())
                        {
                            if (Pricing.RoundDecimal(userAssetBase.Available) != 0) context.PlaceSellOrderAsync(Math.Round(userAssetBase.Available, 4, MidpointRounding.ToZero), marketDataContext.GetRTDTicker().LastPrice.Value, CommonOrderSide.Sell, CommonOrderType.Limit, null);
                        }

                    }
                    if (userTargetAsset != null)
                    {
                        var maxBuyQuantity = Pricing.MaxBuyQuantity(marketDataContext.GetRTDTicker().LastPrice.Value, userTargetAsset.Available);
                        if (marketDataContext.GetRTDTicker().LastPrice.Value <= marketDataContext.GetBestBuyPrice() && maxBuyQuantity !=0 )
                        {
                            var price = marketDataContext.GetRTDTicker().LastPrice.Value;
                            var total = Pricing.GetPrice(maxBuyQuantity, price);         
                                context.PlaceOrderBuyAsync(maxBuyQuantity, price, null);
                                marketDataContext.lastInvested = price;
                        }
                    }
                }
            }

        }

        public override IBinanceTick Handle(IBinanceTick tick)
        {
            logger.LogDebug(Ticker.BestAskPrice + " || " + Ticker.BestAskQuantity);
            return tick;
        }


    }
}
