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
using BinanceAPI.NET.Core.Models.Socket.Clients;
using BinanceAPI.NET.Core.Models.Objects.StreamData;

namespace BinanceAPI.NET.Core.Models
{
    public class BinanceMarketDataService
    {
        public ILoggerFactory LoggerFactory { get; set; }
        public ISocketConfiguration Configuration { get; set; }
        public ConcurrentDictionary<BinanceStreamType, Dictionary<string,IBinanceStreamData>> StreamData = new();
        public BinanceKlineCandlestickStream KlineCandlestickStream { get; set; }
        public BinanceTickerStream TickerStream { get; set; }
        public BinanceMiniTickerStream MiniTickerStream { get; set; }
        public BinanceBookTickerStream BookTickerStream { get; set; }
        public BinancePartialBookDepthStream PartialBookDepthStream { get; set; }
        public BinanceRollingWindowStatsStream RollingWindowStatsStream { get; set; }
        public BinanceTradeStream TradeStream { get; set; }
        public BinanceAggregateTradeStream AggregateTradeStream { get; set; }

        private IWebSocketService<BinanceWebSocketRequestMessage> Client { get; set; }
        private uint lastRequestId = 0;

        public CancellationTokenSource TokenSource { get; set; }

        public BinanceMarketDataService(ILoggerFactory loggerFactory,SocketConfiguration socketConfiguration, CancellationTokenSource tokenSource)
        {
            Configuration = socketConfiguration;
            LoggerFactory= loggerFactory;
            Client = new WebSocketService<BinanceWebSocketRequestMessage>(socketConfiguration, loggerFactory, tokenSource);
            TokenSource = tokenSource;
            Initialize();
        }

       

        internal void SubscribeAsync(string[] streams)
        {
            var request = new BinanceWebSocketRequestMessage(NextId(), BinanceRequestMessageType.Subscribe, streams);
            Client.SendRequestAsync(request);
        }

        internal void SubscribeAsync(string stream)
        {
            var request = new BinanceWebSocketRequestMessage(NextId(), BinanceRequestMessageType.Subscribe, new string[] { stream });
            Client.SendRequestAsync(request);
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
            Client.OnError += OnError;
            Client.OnClose += OnClose;
            Client.OnReconnected += OnReconnected;
            Client.OnReconnecting += OnReconnecting;
            Client.OnMessage += OnMessage;
            Client.OnOpen += OnOpen;
            
            KlineCandlestickStream = new (this);
            TickerStream = new (this);
            MiniTickerStream= new (this);
            BookTickerStream= new (this);
            PartialBookDepthStream= new(this);
            RollingWindowStatsStream= new(this);
            TradeStream= new(this);
            AggregateTradeStream= new(this);

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

        public virtual void OnMessage(byte[] streamData)
        {
            DispatchMessage(Deserialize(streamData, IBinanceStreamData.GetSerializationSettings()).Result.EventType,streamData);
        }

        private void DispatchMessage(BinanceEventType type, byte[] streamData)
        {
            switch (type)
            {
                case BinanceEventType.Kline:
                    var data = (BinanceKlineCandlestickData)JsonConvert.DeserializeObject<BinanceKlineCandlestickData>(Configuration.Encoding.GetString(streamData),IBinanceStreamData.GetSerializationSettings());
                    if(data.Symbol != null)
                    {
                        GetStreamData(BinanceStreamType.KlineCandlestick).Remove(data.Symbol);
                        GetStreamData(BinanceStreamType.KlineCandlestick).Add(data.Symbol, data);
                    }
                        break;
            }
        }


        public void OnReconnecting()
        {

        }

        public void OnReconnected()
        {

        }

        public Dictionary<string,IBinanceStreamData> GetStreamData(BinanceStreamType streamType)

        {
            Dictionary<string, IBinanceStreamData> result;
            StreamData.TryGetValue(streamType, out result);
            return result;
        }

        public IBinanceStreamData GetStreamData(BinanceStreamType streamType, string symbol)
        {
            GetStreamData(streamType).TryGetValue(symbol, out IBinanceStreamData result);
            return result;
        }

        private Task<BinanceStreamData?> Deserialize(byte[] message, JsonSerializerSettings Options)
        {
            var json = Configuration.Encoding.GetString(message);
            var obj = JsonConvert.DeserializeObject<BinanceStreamData>(json, Options);
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