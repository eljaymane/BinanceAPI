
using Binance.Spot;
using Binance.Spot.Models;
using BinanceBOT;
using BinanceBOT.Core.Model;
using BinanceBOT.Core.Model.Abstractions;
using BinanceBOT.Core.Model.Market;
using BinanceBOT.Core.Model.WalletContext;
using BinanceBOT.DataWatchers;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System.Text.Json;
using System.Text.Json.Nodes;

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
             configuration.AddUserSecrets<GlobalConfig>();
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
                    .AddTransient<GlobalConfig>()
                    .AddTransient<BaseClient>()
                    .AddTransient<TickerWatcherFactory>()
                    .AddSingleton<ContextFactory>()
                    .AddSingleton<OrderWatcherFactory>()
                    .AddSingleton<TradingService>()
                    );



var host = builder.Build();
var client = host.Services.GetRequiredService<BaseClient>();
var config = host.Services.GetRequiredService<GlobalConfig>();
var marketFactory = host.Services.GetRequiredService<ContextFactory>();
var orderFactory = host.Services.GetRequiredService<OrderWatcherFactory>();
var factory = host.Services.GetRequiredService<TickerWatcherFactory>();


var service = host.Services.GetRequiredService<TradingService>();
service.Initialize("ETH", "BUSD");

OrderId OrderP(WebCallResult<OrderId> result)
{
    if (result.Data == null) return null;
    Console.WriteLine(result.Data.Id);
    return result.Data;
}