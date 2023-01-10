# Binance Websocket API
This project is a simple implementation of binance websocket streams. It's purpose is fully educational.
It's based on a simple asynchronous websocket client and can handle subscription to multiple streams.
It does also provide a centralized access to the data resulting from subscriptions.

## Core
This represents the binance implementation of the WebSocket communication. Anything related to binance is found there.
## Infrastructure
This represents the technical part of the implementation. This holds a simple asynchronous client, heavily commented to make it easier to understand. The same infrastructure could be used for any other asynchronous WebSocket implementation.
## Usage 
```C#
var configuration = new SocketConfiguration(new Uri("wss://stream.binance.com/stream"), true);
//Assuming that you know how to get the loggerFactory.
BinanceMarketDataService client = new(loggerFactory,configuration,new CancellationTokenSource());
client.KlineCandlestickStream.SubscribeAsync(KlineInterval.ThreeMinutes, "BTCBUSD");
client.TickerStream.SubscribeAsync("BTCBUSD");

var threadData = new Thread(() => {
    while(true){
        try{
            BinanceKlineCandlestickData kline = (BinanceKlineCandlestickData)client.GetStreamData(BinanceEventType.Kline,"BTCBUSD");
                BinanceTickerData ticker = (BinanceTickerData)client.GetStreamData(BinanceEventType.TwentyFourHourTicker, "BTCBUSD");
                if (kline != null) Console.WriteLine(kline.Data.ClosePrice);
                if (ticker != null) Console.WriteLine(ticker.BestAskPrice);
        } catch(Exception){}
        finally{
            Thread.Sleep(3000);
        }
    }
});

threadData.Start();
```

### Disclaimer 
Not all streams are implemented, the purpose is fully educational !
