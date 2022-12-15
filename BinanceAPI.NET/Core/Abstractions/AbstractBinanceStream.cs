﻿using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Objects;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Text.Json;

namespace BinanceAPI.NET.Core.Abstractions
{
    public abstract class AbstractBinanceStream<T> where T : IBinanceStreamData
    {
        private uint lastRequestId = 0;
        private ILoggerFactory _loggerFactory;
        private CancellationTokenSource ctSource;
        public string? Name { get; set; }
        public BinanceStreamType StreamType { get; private set; }
        public IWebSocketService<BinanceWebSocketRequestMessage> Client { get; private set; }
        public SocketConfiguration Configuration { get; private set; }

        internal SemaphoreSlim dataSem = new SemaphoreSlim(1,1);
        internal T data { get; set; }

        public AbstractBinanceStream(BinanceStreamType streamType,SocketConfiguration configuration,ILoggerFactory loggerFactory,CancellationTokenSource ctSource)
        {
            StreamType = streamType;
            Client = new WebSocketService<BinanceWebSocketRequestMessage>(configuration, loggerFactory, ctSource);
            Configuration = configuration;
        }

        public Task SubscribeAsync(string[] streams)
        {
            var request = new BinanceWebSocketRequestMessage(NextId(),BinanceRequestMessageType.Subscribe, streams);
            Client.SendRequestAsync(request);
            return Task.CompletedTask;
        }

        public Task SubscribeAsync(string stream)
        {
            var request = new BinanceWebSocketRequestMessage(NextId(), BinanceRequestMessageType.Subscribe, new string[] {stream});
            Client.SendRequestAsync(request);
            return Task.CompletedTask;
        }

        public Task UnSubscribe(string stream)
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


        public abstract void Initialize();

        public abstract void OnError(Exception exception);

        public abstract void OnClose();

        public abstract void OnOpen();

        public virtual void OnMessage(byte[] streamData)
        {
            data = Deserialize(streamData,IBinanceStreamData.GetSerializationSettings()).Result;
        }

        public virtual Task<T?> Deserialize(byte[] message,JsonSerializerSettings Options)
        {
            var json = Configuration.Encoding.GetString(message);
            var obj = JsonConvert.DeserializeObject<T>(json,Options);
            return Task.FromResult(obj);
        }

        public virtual Task<byte[]> Serialize(T obj, JsonSerializerOptions serializerOptions)
        {
            var json = System.Text.Json.JsonSerializer.Serialize<T>(obj, serializerOptions);
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            return Task.FromResult(jsonBytes);
        }

        public abstract void OnReconnecting();

        public abstract void OnReconnected();

        public T GetStreamData()
        {
            return data;
        }

        public uint NextId()
        {
            return lastRequestId++;
        }
    }
}
