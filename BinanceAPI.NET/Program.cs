// See https://aka.ms/new-console-template for more information
using BinanceAPI.NET.Core.Models;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Core.Stats;
using BinanceAPI.NET.Core.Stats.Historical.Klines;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PEzbus;
using System.Reflection.Metadata.Ecma335;

static void ConfigureServices(IServiceCollection services)
{
    services.AddLogging(configure => configure.AddConsole());



}


static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        })
         .ConfigureAppConfiguration(configuration =>
         {
         
         });


}
var services = new ServiceCollection();
services
    .AddSingleton<IPEzEventBus,PEzEventBus>()
    .AddScoped<BinanceHistoricalKlineService>();

ConfigureServices(services);
var serviceProvider = services.BuildServiceProvider();


using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
        .AddConsole();
    
});
var builder = CreateHostBuilder(args).ConfigureServices((_, services) => services
            .AddLogging(configure =>
            configure.AddConsole()
            ));



var host = builder.Build();

var uri = new Uri("wss://stream.binance.com:9443/ws");
var configuration = new SocketConfiguration(uri, true);
var eventBus = serviceProvider.GetRequiredService<IPEzEventBus>();

BinanceHistoricalKlineService service = serviceProvider.GetRequiredService<BinanceHistoricalKlineService>();
BinanceMarketStats stats = new("BTCUSDT");
eventBus.Register(stats);
eventBus.Register(service);
service.DonwloadAndProcess("BTCUSDT", "2024", "05", KlineInterval.FourHours);
service.DonwloadAndProcess("BTCUSDT", "2024", "06", KlineInterval.FourHours);
BinanceMarketDataService client = new(loggerFactory,configuration,new CancellationTokenSource(),eventBus);
//client.KlineCandlestickStream.SubscribeAsync("BTCUSDT", KlineInterval.OneSecond);
await client.KlineCandlestickStream.SubscribeAsync("BTCUSDT",KlineInterval.FourHours);
var threadData = new Thread(() =>
{
    while (true)
    {
        try
        {
           // BinanceKlineCandlestickData data = (BinanceKlineCandlestickData)client.GetStreamData(BinanceStreamType.KlineCandlestick,"BTCUSDT");
            BinanceKlineCandlestickData data1 = (BinanceKlineCandlestickData)client.GetStreamData(BinanceStreamType.KlineCandlestick,"BTCUSDT");
            Console.WriteLine(stats.GetStats());
            //if (data != null) Console.WriteLine($"Kline BTCUSDT : {data.Data.ClosePrice}");
            if (data1 != null) Console.WriteLine($"Kline BTCUSDT : {data1.Data.HighPrice}");
        }
        catch (Exception) { }
        finally { Thread.Sleep(2000); }

    }


});

threadData.Start();

