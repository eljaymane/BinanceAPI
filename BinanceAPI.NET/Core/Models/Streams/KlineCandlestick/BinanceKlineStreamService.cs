using BinanceAPI.NET.Core.Abstractions;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Socket;
using BinanceAPI.NET.Core.Models.Socket.Clients;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

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
        }

        public void SubscribeAsync(KlineInterval interval,string symbol)
        {
            var request = new BinanceWebSocketRequestMessage(0,
                BinanceRequestMessageType.Subscribe, new string[] { symbol.ToLower() + "@" + interval.GetStringValue()});
            Client.SendRequestAsync(request);
        }

        public Task<BinanceKlineCandlestickData> GetKlineDataAsync()
        {
            dataSem.Wait();
            return Task.FromResult(data);
        }

        public BinanceKlineStreamService SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            return this;
        }

        public override void Initialize()
        {
            Client.OnError += OnError;
            Client.OnClose+= OnClose;
            Client.OnReconnected+= OnReconnected;
            Client.OnReconnecting+= OnReconnecting;
            Client.OnMessage += OnMessage;
            Client.OnOpen+= OnOpen;
            Client.Start();
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

        public override void OnMessage(dynamic message)
        {
            dataSem.Wait();
            data = message;
            dataSem.Release();
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
