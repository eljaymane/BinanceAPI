﻿using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration;

namespace BinanceAPI.NET.Core.Abstractions
{
    public abstract class AbstractBinanceStream<T> where T : IBinanceStreamData
    {
        private uint lastRequestId = 0;
        private CancellationTokenSource ctSource;
        public string? Name { get; set; }
        public BinanceStreamType StreamType { get; private set; }
        public BinanceMarketDataService Client { get; private set; }
        public SocketConfiguration Configuration { get; private set; }

        internal SemaphoreSlim dataSem = new SemaphoreSlim(1,1);
        internal T data { get; set; }

        public AbstractBinanceStream(ref BinanceMarketDataService client, BinanceStreamType streamType)
        {
            Client = client;
            StreamType = streamType;
            CreateDataStore();
        }

        private void CreateDataStore()
        {
            if (!Client.StreamData.ContainsKey(StreamType)) Client.StreamData.TryAdd(StreamType, new Dictionary<string, IBinanceStreamData>());
        }
    }
}
