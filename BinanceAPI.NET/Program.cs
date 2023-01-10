// See https://aka.ms/new-console-template for more information
using BinanceAPI.NET.Core.Models;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
var serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection);
var serviceProvider = serviceCollection.BuildServiceProvider();


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
            )
                    
                    );



var host = builder.Build();


var configuration = new SocketConfiguration(new Uri("wss://stream.binance.com/stream"), true);
BinanceMarketDataService client = new(loggerFactory,configuration,new CancellationTokenSource());
client.KlineCandlestickStream.SubscribeAsync(KlineInterval.ThreeMinutes, "BTCBUSD");
client.TickerStream.SubscribeAsync("BTCBUSD");
var threadData = new Thread(() =>
{
    while (true)
    {
        try
        {
            BinanceKlineCandlestickData data = (BinanceKlineCandlestickData)client.GetStreamData(BinanceEventType.Kline,"BTCBUSD");
            BinanceTickerData dat = (BinanceTickerData)client.GetStreamData(BinanceEventType.TwentyFourHourTicker, "BTCBUSD");
            if (data != null) Console.WriteLine(data.Data.ClosePrice);
            if (dat != null) Console.WriteLine(dat.BestAskPrice);
        }
        catch (Exception) { }
        finally { Thread.Sleep(4000); }

    }


});

threadData.Start();

