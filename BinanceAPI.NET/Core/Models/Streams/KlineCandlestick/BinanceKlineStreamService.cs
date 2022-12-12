using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Socket;
using BinanceAPI.NET.Core.Models.Socket.Clients;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Extensions;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BinanceAPI.NET.Core.Models.Streams.KlineCandlestick
{
    public class BinanceKlineStreamService : AbstractBinanceStream<BinanceKlineCandlestickData>
    {
        
        private ILoggerFactory _loggerFactory;
        private CancellationTokenSource ctSource;


        public BinanceKlineStreamService(ILoggerFactory loggerFactory,SocketConfiguration configuration,CancellationTokenSource tokenSource) : base(BinanceStreamType.KlineCandlestick,configuration,loggerFactory,tokenSource)
        {
            _loggerFactory = loggerFactory;
            ctSource = tokenSource;
          Initialize(); 
            Client.Start();
        }

        public void SubscribeAsync(KlineInterval interval,string symbol)
        {
            var request = new BinanceWebSocketRequestMessage(0,
                BinanceRequestMessageType.Subscribe, new string[] { symbol.ToLower() + "@" + interval.GetStringValue()});
            var serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new RequestMessageTypeJsonConverter()
                }
            };
            Client.SendRequestAsync(request,serializerOptions);
        }

        public Task<BinanceKlineCandlestickData> GetKlineDataAsync()
        {
            dataSem.Wait();
            return Task.FromResult((BinanceKlineCandlestickData)data);
        }

        public BinanceKlineStreamService SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            return this;
        }

        public override void Initialize()
        {
            Client.OnError+= OnError;
            Client.OnClose+= OnClose;
            Client.OnReconnected+= OnReconnected;
            Client.OnReconnecting+= OnReconnecting;
            Client.OnMessage += OnMessage;
            Client.OnOpen+= OnOpen;
            //Client.Start();
        }

        public override void OnError(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override void OnClose()
        {
            throw new NotImplementedException();
        }

        public override void OnOpen()
        {
            throw new NotImplementedException();
        }

        public override void OnMessage(byte[] streamData)
        {
            base.OnMessage(streamData);
        }

        public override void OnReconnecting()
        {
            throw new NotImplementedException();
        }

        public override void OnReconnected()
        {
            throw new NotImplementedException();
        }
    }
}
