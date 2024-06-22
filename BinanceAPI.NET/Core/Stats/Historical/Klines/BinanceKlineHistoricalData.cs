using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Helpers;
using BinanceAPI.NET.Core.Models.Objects.Entities;

namespace BinanceAPI.NET.Core.Stats.Historical.Klines
{
    [Serializable]
    public sealed class BinanceKlineHistoricalData : IBinanceHistoricalData
    {
        public DateTime OpenTime { get; }
        public decimal Open { get; }
        public decimal High { get; }
        public decimal Low { get; }
        public decimal Close { get; }
        public decimal Volume { get; }
        public DateTime CloseTime { get; }
        public decimal QuoteAssetVolume { get; }
        public long NumberOfTrades { get; }
        public decimal TakerBuyBaseAssetVolume { get; }
        public decimal TakerBuyQuoteAssetVolume { get; }
        public BinanceKlineHistoricalData(string openTime, string open, string high, string low,
               string close, string volume, string closeTime, string quoteAssetVolume, string numberOfTrades,
               string takerBuyBaseAssetVolume, string takerBuyQuoteAssetVolume)
        {
            OpenTime = Calculation.EpochToDate(long.Parse(openTime));
            Open = decimal.Parse(open.Replace('.', ','));
            High = decimal.Parse(high.Replace('.', ','));
            Low = decimal.Parse(low.Replace('.', ','));
            Close = decimal.Parse(close.Replace('.', ','));
            Volume = decimal.Parse(volume.Replace('.', ','));
            CloseTime = Calculation.EpochToDate(long.Parse(closeTime));
            QuoteAssetVolume = decimal.Parse(quoteAssetVolume.Replace('.', ','));
            NumberOfTrades = long.Parse(numberOfTrades);
            TakerBuyBaseAssetVolume = decimal.Parse(takerBuyBaseAssetVolume.Replace('.', ','));
            TakerBuyQuoteAssetVolume = decimal.Parse(takerBuyQuoteAssetVolume.Replace('.', ','));
        }
        public BinanceKlineHistoricalData(BinanceKlineCandlestick data)
        {
            OpenTime = data.StartTime;
            CloseTime = data.EndTime;
            Open = data.OpenPrice;
            Close = data.ClosePrice;
            Volume = data.BaseAssetVolume;
            NumberOfTrades = data.NumberOfTrades;
            TakerBuyBaseAssetVolume = data.TakerBuyBaseVolume;
            TakerBuyQuoteAssetVolume = data.TakerBuyQuoteVolume;
        }
    }
}
