using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Socket;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace BinanceAPI.NET.Core.Abstractions
{
    public abstract class AbstractBinanceStream<T> where T : IBinanceStreamData
    {
        public string? Name { get; set; }
        public BinanceStreamType StreamType { get; private set; }
        public IWebSocketService<BinanceWebSocketRequestMessage> Client { get; private set; }

        internal SemaphoreSlim dataSem = new SemaphoreSlim(1,1);
        internal T? data { get; set; }

        public AbstractBinanceStream(BinanceStreamType streamType,SocketConfiguration configuration,ILoggerFactory loggerFactory,CancellationTokenSource ctSource)
        {
            StreamType = streamType;
            Client = new WebSocketService<BinanceWebSocketRequestMessage>(configuration, loggerFactory, ctSource);
        }


        public abstract void Initialize();

        public abstract void OnError(Exception exception);

        public abstract void OnClose();

        public abstract void OnOpen();

        public abstract void OnMessage(dynamic message);

        public abstract void OnReconnecting();

        public abstract void OnReconnected();
    }
}
