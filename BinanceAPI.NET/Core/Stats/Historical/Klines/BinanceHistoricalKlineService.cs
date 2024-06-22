using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Infrastructure.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Compression;

using System.Net;
using PEzbus;
using BinanceAPI.NET.Core.Stats.Historical.Klines.Events;
using BinanceAPI.NET.Core.Stats.Historical.Klines.events;
using PEzbus.CustomAttributes;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace BinanceAPI.NET.Core.Stats.Historical.Klines
{
    public class BinanceHistoricalKlineService : IBinanceHistoricalKlineService
    {
        private readonly string baseUrlMonthly = @"https://data.binance.vision/data/spot/monthly/klines";
        private readonly string baseUrlDaily = @"https://data.binance.vision/data/spot/daily/klines";
        private readonly string currentDirectory = AppDomain.CurrentDomain.BaseDirectory + "/";
        private readonly string fileNameMonthly = "{symbol}-{interval}-{year}-{month}.zip";
        private readonly string fileNameDaily = "{symbol}-{interval}-{year}-{month}-{day}.zip";
        private const string defaultDir = "historical_kline/";

        private readonly IPEzEventBus _eventBus;

        private ConcurrentDictionary<string, Dictionary<DateTime, BinanceKlineHistoricalData>> Data;

        public BinanceHistoricalKlineService(IPEzEventBus eventBus)
        {
            _eventBus = eventBus;
            Data = new();
        }

        [Subscribe(typeof(KlineDataReceivedEvent))]
        public void LoadData(KlineDataReceivedEvent @event)
        {
            AddData(@event.Symbol, @event.KlineData);
        }

        public IEnumerable<IBinanceHistoricalData> GetMostRecentData(string symbol, int periods)
        {
            if(!Data.TryGetValue(symbol, out var historicalData)) return Array.Empty<IBinanceHistoricalData>();
            var keys = historicalData.Keys.OrderByDescending(x => x).Take(periods).ToList();
            return historicalData.Values.Where(x => keys.Any(y => y == x.OpenTime));
        }

        private string GetFileNameMonthly(string symbol, string year, string month, KlineInterval interval)
        {
            var name = fileNameMonthly.Replace("{symbol}", symbol).Replace("{interval}", interval.GetStringValue())
                .Replace("{year}", year).Replace("{month}", month);
            return name;
        }

        private IEnumerable<string> GetFileNameDaily(string symbol, string year, string month, KlineInterval interval)
        {
            for (int i = 1; i <= DateTime.DaysInMonth(int.Parse(year), MonthToNum(month)) ; i++)
            {
                if(i >= DateTime.Now.Day) yield break;
                var name = fileNameDaily.Replace("{symbol}", symbol).Replace("{interval}", interval.GetStringValue())
                .Replace("{year}", year).Replace("{month}", month);
                if(i < 10) name = name.Replace("{day}", "0"+i.ToString());
                else name = name.Replace("{day}", i.ToString());
                yield return name;
            }
        }

        public async void DonwloadAndProcess(string symbol, string year, string month, KlineInterval interval)
        {
            var isCurrentMonth = IsCurrentMonth(month);
            try
            {
                if(isCurrentMonth) await DownloadDaily(symbol, year, month, interval);
                else await DownloadMonthly(symbol, year, month, interval);
                await Process(symbol,year,month,interval);
            }
            catch (WebException ex)
            {

            }
        }


        private bool IsCurrentMonth(string month)
        {
            var monthNum = MonthToNum(month);
            var currentMonth = DateTime.Now.Month;
            return monthNum == currentMonth;
        }

        private int MonthToNum(string month)
        {
            return int.Parse(month);
        }

        private async Task DownloadMonthly(string symbol, string year, string month, KlineInterval interval)
        {
            var url = baseUrlMonthly + "/{symbol}/{interval}/";
            url = url.Replace("{symbol}", symbol).Replace("{interval}", interval.GetStringValue());
            url = url + GetFileNameMonthly(symbol, year, month, interval);
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, url.Split('/')[9]);
                }
            }
            catch (WebException ex)
            {
            }
           
        }

        private async Task DownloadDaily(string symbol, string year, string month, KlineInterval interval)
        {
            var url = baseUrlDaily + "/{symbol}/{interval}/";
            url = url.Replace("{symbol}", symbol).Replace("{interval}", interval.GetStringValue());
            //url = url + GetFileNameMonthly(symbol, year, month, interval);
            foreach (var file in GetFileNameDaily(symbol, year, month, interval))
            {
                var _url = url + file;
                try
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(_url, _url.Split('/')[9]);
                    }
                }
                catch (WebException ex)
                {
                }
            }
           

        }

        private async Task Process(string symbol,string year, string month, KlineInterval interval)
        {
            var isCurrentMonth = IsCurrentMonth(month);
            if(isCurrentMonth) await ProcessDaily(GetFileNameDaily(symbol, year, month, interval));
            else await ProcessMonthly(GetFileNameMonthly(symbol, year, month, interval));          
        }

        private async Task ProcessMonthly(string filename)
        {
            FileStream fs = new FileStream(currentDirectory + filename, FileMode.Open);
            if (!Directory.Exists(currentDirectory + defaultDir)) Directory.CreateDirectory(currentDirectory + defaultDir);
            ZipFile.ExtractToDirectory(fs, currentDirectory + defaultDir, true);
            await LoadData(filename.Split('.')[0]);
        }

        private async Task ProcessDaily(IEnumerable<string> filenames)
        {
            foreach (string filename in filenames)
            {
                FileStream fs = new FileStream(currentDirectory + filename, FileMode.Open);
                if (!Directory.Exists(currentDirectory + defaultDir)) Directory.CreateDirectory(currentDirectory + defaultDir);
                ZipFile.ExtractToDirectory(fs, currentDirectory + defaultDir, true);
                await LoadData(filename.Split('.')[0]);
            }
            
        }

        private Task LoadData(string filename)
        {
            filename += ".csv";
            var symbol = filename.Split('-')[0];
            var csvData = File.ReadAllText(currentDirectory + defaultDir + filename);
            var dataArray = csvData.Split('\n');
            foreach (var data in dataArray)
            {
                var values = data.Split(',');
                if (values[0] == "") continue;
                var klineData = new BinanceKlineHistoricalData(values[0], values[1], values[2], values[3], values[4],
                    values[5], values[6], values[7], values[8], values[9], values[10]);
                AddData(symbol, klineData);

            }
            return Task.CompletedTask;
        }

        private void AddData(string symbol, BinanceKlineHistoricalData klineData)
        {
            Data.TryAdd(symbol, new Dictionary<DateTime, BinanceKlineHistoricalData>());
            Data.TryGetValue(symbol, out var historicalList);
            if (historicalList.ContainsKey(klineData.OpenTime)) return;
            historicalList?.Add(klineData.OpenTime, klineData);
            var latestData = historicalList.Values.OrderByDescending(x => x.OpenTime).Take(BinanceMarketStats.defaultMacdPeriods).ToList();
            _eventBus.Publish(new KlineHistoricalDataAddedEvent(symbol, latestData));
        }
    }
}
