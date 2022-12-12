﻿using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Socket;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

namespace BinanceAPI.NET.Core.Abstractions
{
    public abstract class AbstractBinanceStream<T> where T : IResponseDataType
    {
        public string? Name { get; set; }
        public BinanceStreamType StreamType { get; private set; }
        public IWebSocketService<BinanceWebSocketRequestMessage> Client { get; private set; }
        public SocketConfiguration Configuration { get; private set; }

        internal SemaphoreSlim dataSem = new SemaphoreSlim(1,1);
        internal IResponseDataType? data { get; set; }

        public AbstractBinanceStream(BinanceStreamType streamType,SocketConfiguration configuration,ILoggerFactory loggerFactory,CancellationTokenSource ctSource)
        {
            StreamType = streamType;
            Client = new WebSocketService<BinanceWebSocketRequestMessage>(configuration, loggerFactory, ctSource);
            Configuration = configuration;
        }


        public abstract void Initialize();

        public abstract void OnError(Exception exception);

        public abstract void OnClose();

        public abstract void OnOpen();

        public abstract void OnMessage(byte[] streamData);

        public virtual Task<T?> Deserialize(byte[] message,JsonSerializerOptions serializerOptions)
        {
            var json = Configuration.Encoding.GetString(message);
            var obj = JsonSerializer.Deserialize<T>(json,serializerOptions);
            return Task.FromResult(obj);
        }

        public virtual Task<byte[]> Serialize(T obj, JsonSerializerOptions serializerOptions)
        {
            var json = JsonSerializer.Serialize<T>(obj, serializerOptions);
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            return Task.FromResult(jsonBytes);
        }

        public abstract void OnReconnecting();

        public abstract void OnReconnected();
    }
}
