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

var uri = new Uri("wss://stream.binance.com:9443/ws");
var configuration = new SocketConfiguration(uri, true);
BinanceMarketDataService client = new(loggerFactory,configuration,new CancellationTokenSource());
client.KlineCandlestickStream.SubscribeAsync(KlineInterval.OneSecond, "BTCUSDT");
var threadData = new Thread(() =>
{
    while (true)
    {
        try
        {
            BinanceKlineCandlestickData data = (BinanceKlineCandlestickData)client.GetStreamData(BinanceStreamType.KlineCandlestick,"BTCUSDT");
            if (data != null) Console.WriteLine(data.Data.ClosePrice);
        }
        catch (Exception) { }
        finally { Thread.Sleep(2000); }

    }


});

threadData.Start();

