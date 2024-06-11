using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects;
using BinanceAPI.NET.Core.Models.Streams;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text;
using System.Collections.Concurrent;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models.Objects.StreamData;
using System.IO;
using BinanceAPI.NET.Core.Models.Objects.Entities;

namespace BinanceAPI.NET.Core.Models
{
    /// <summary>
    /// The service that provides access to Binance Websocket streams. It does also hold the data coming from any particular stream.
    /// Data can be accessed from StreamData using the event type and instrument ex : BTCBUSD.
    /// To subscribe to a stream, the corresponding member should be used, the data in the other hand is accessed in a centralized way.
    /// </summary>
    public class BinanceMarketDataService
    {
        public ILoggerFactory LoggerFactory { get; set; }
        /// <summary>
        /// The configuration used for the websocket connection underneath the service.
        /// </summary>
        public ISocketConfiguration Configuration { get; set; }
        /// <summary>
        /// The centralized place where all the upcoming data fall into.
        /// </summary>
        public ConcurrentDictionary<BinanceStreamType, Dictionary<string,IBinanceStreamData>> StreamData = new();
        public List<string> Subscriptions;
        public BinanceKlineCandlestickStream KlineCandlestickStream { get; set; }
        public BinanceTickerStream TickerStream { get; set; }
        public BinanceMiniTickerStream MiniTickerStream { get; set; }
        public BinanceBookTickerStream BookTickerStream { get; set; }
        public BinancePartialBookDepthStream PartialBookDepthStream { get; set; }
        public BinanceRollingWindowStatsStream RollingWindowStatsStream { get; set; }
        public BinanceTradeStream TradeStream { get; set; }
        public BinanceAggregateTradeStream AggregateTradeStream { get; set; }
        public CancellationTokenSource TokenSource { get; set; }
        private IWebSocketService<BinanceWebSocketRequestMessage> Client { get; set; }
        private uint lastRequestId = 0;
        private uint lastSubscriptionId;
        private event EventHandler SubscripionsChanged;

        

        public BinanceMarketDataService(ILoggerFactory loggerFactory,SocketConfiguration socketConfiguration, CancellationTokenSource tokenSource)
        {
            Configuration = socketConfiguration;
            LoggerFactory= loggerFactory;
            Client = new WebSocketService<BinanceWebSocketRequestMessage>(socketConfiguration, loggerFactory, tokenSource);
            TokenSource = tokenSource;
            Subscriptions = new();
            Initialize();
        }

       protected static void OnSubscriptionChanged()
        {

        }

        public Task UnsubscribeAll()
        {
            string[] payload = new string[Subscriptions.Count];
            int i = 0;
            foreach (var stream in Subscriptions)
            {
                payload[i] = stream;
                i++;
            }
            
            var request = new BinanceWebSocketRequestMessage(lastSubscriptionId, BinanceRequestMessageType.Unsubscribe, payload);
            Client.SendRequestAsync(request).Wait();
            Thread.Sleep(2000); //Little workaround to wait the server to unsubscribe.
            return Task.CompletedTask;
        }

        private Task SubscribeAll()
        {
            string[] payload = new string[Subscriptions.Count];
            int i = 0;
            foreach (var stream in Subscriptions)
            {
                payload[i] = stream;
                i++;
            }
            //payload[i] = "btcbusd@ticker";
            lastSubscriptionId = NextId();
            var request = new BinanceWebSocketRequestMessage(lastSubscriptionId, BinanceRequestMessageType.Subscribe, payload);
            Client.SendRequestAsync(request).Wait();
            return Task.CompletedTask;
        }

        internal Task SubscribeAsync(string[] streams)
        {
            if(Subscriptions.Count > 0) UnsubscribeAll().Wait();
            foreach (var stream in streams)
            {
                Subscriptions.Add(stream);
            }
            SubscribeAll();
            return Task.CompletedTask;
        }

        internal Task SubscribeAsync(string stream)
        {
            if(Subscriptions.Count > 0) UnsubscribeAll().Wait();
            Subscriptions.Add(stream);
            SubscribeAll().Wait();
            return Task.CompletedTask;
        }

        private Task UnSubscribe(string stream)
        {
            var request = new BinanceWebSocketRequestMessage(NextId(), BinanceRequestMessageType.Unsubscribe, new string[] { stream });
            Client.SendRequestAsync(request);
            return Task.CompletedTask;
        }

        public Task ListSubscriptions()
        {
            var request = new BinanceWebSocketRequestMessage(NextId(), BinanceRequestMessageType.ListSubscriptions, new string[] { "" });
            Client.SendRequestAsync(request);
            return Task.CompletedTask;
        }
        public void Initialize()
        {
            /// Actions that handle the socket client events.
            Client.OnError += OnError;
            Client.OnClose += OnClose;
            Client.OnReconnected += OnReconnected;
            Client.OnReconnecting += OnReconnecting;
            Client.OnMessage += OnMessage;
            Client.OnOpen += OnOpen;
            
            ///Creating new instances of the stream object which are more of a request factory that holds a reference to this service
            /// in a way that all the upcoming data falls into one single place.
            KlineCandlestickStream = new (this);
            TickerStream = new (this);
            MiniTickerStream= new (this);
            BookTickerStream= new (this);
            PartialBookDepthStream= new(this);
            RollingWindowStatsStream= new(this);
            TradeStream= new(this);
            AggregateTradeStream= new(this);

            ///Socket thread.
            var socket = new Thread(() => {
                Client.Start();
            });
            socket.Start();
        }
        public void OnError(Exception exception)
        {

        }

        public void OnClose()
        {

        }

        public void OnOpen()
        {

        }

        public virtual async void OnMessage(byte[] streamData)
        {
            var eventType = await Deserialize(streamData, IBinanceStreamData.GetSerializationSettings());
            if(eventType.Data != null){
                DispatchMessage(eventType.Data.EventType, streamData);
            }
            
        }

        private void DispatchMessage(BinanceEventType type, byte[] streamData)
        {
            
                switch (type)
                {
                    case BinanceEventType.Kline:
                        var json = Configuration.Encoding.GetString(streamData);
                        var kline = JsonConvert.DeserializeObject<BinanceKlineCandlestickData>(json, IBinanceStreamData.GetSerializationSettings());
                        GetStreamData(BinanceStreamType.KlineCandlestick).Remove(kline.Symbol);
                        GetStreamData(BinanceStreamType.KlineCandlestick).Add(kline.Symbol, kline);
                        break;
                    default: break;
                }
        }


        public void OnReconnecting()
        {

        }

        public void OnReconnected()
        {

        }
        /// <summary>
        /// The method that returns a dictionary holding streams of different instruments of the same type (stream type)
        /// </summary>
        /// <param name="streamType"></param>
        /// <returns></returns>

        public Dictionary<string,IBinanceStreamData> GetStreamData(BinanceStreamType streamType)

        {
            Dictionary<string, IBinanceStreamData> result;
            StreamData.TryGetValue(streamType, out result);
            return result;
        }
        /// <summary>
        /// The method that returns the desired stream data
        /// </summary>
        /// <param name="streamType">Refer to BinanceEventType</param>
        /// <param name="symbol">Desired symbol ex : BTCBUSD</param>
        /// <returns></returns>
        public IBinanceStreamData GetStreamData(BinanceStreamType streamType, string symbol)
        {
            GetStreamData(streamType).TryGetValue(symbol, out IBinanceStreamData result);
            return result;
        }
        /// <summary>
        /// The message that turns bytes into a BinanceStreamResponse object.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="Options"></param>
        /// <returns></returns>
        private Task<BinanceStreamResponse<BinanceStreamData>> Deserialize(byte[] message, JsonSerializerSettings Options)
        {
            var json = Configuration.Encoding.GetString(message);
             var obj = JsonConvert.DeserializeObject<BinanceStreamResponse<BinanceStreamData>>(json, Options);
            return Task.FromResult(obj);
        }

        private Task<byte[]> Serialize(BinanceWebSocketRequestMessage obj, JsonSerializerOptions serializerOptions)
        {
            var json = System.Text.Json.JsonSerializer.Serialize<BinanceWebSocketRequestMessage>(obj);
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            return Task.FromResult(jsonBytes);
        }
        private uint NextId()
        {
            return lastRequestId++;
        }

    }
}