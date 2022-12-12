﻿// See https://aka.ms/new-console-template for more information
using BinanceAPI.NET.Core.Models;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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


var configuration = new SocketConfiguration(new Uri("wss://stream.binance.com/ws"), true);
BinanceMarketDataService client = new(loggerFactory,configuration,new CancellationTokenSource());
client.KlineStream?.SubscribeAsync(KlineInterval.FifteenMinutes,"BTCBUSD");

var threadData = new Thread(async () =>
{
    while (true) 
    {
        var data = await client.KlineStream?.GetKlineDataAsync()!;
        Console.WriteLine(data);
        Thread.Sleep(4000);
    }

    
});

threadData.Start();

