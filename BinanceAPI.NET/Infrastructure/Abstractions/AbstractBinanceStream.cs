using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Socket;
using BinanceAPI.NET.Core.Models.Streams.KlineCandlestick;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using BinanceAPI.NET.Infrastructure.Extensions;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace BinanceAPI.NET.Infrastructure.Abstractions
{
    public abstract class AbstractBinanceStream<T> where T : IBinanceStreamData
    {
        public string? Name { get; set; }
        public BinanceStreamType StreamType { get; private set; }
        public IWebSocketService<BinanceWebSocketRequestMessage> Client { get; private set; }

        internal T? data { get; set; }

        public AbstractBinanceStream(BinanceStreamType streamType,IWebSocketService<BinanceWebSocketRequestMessage> client) 
        {
            StreamType = streamType;
            Client = client;
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
