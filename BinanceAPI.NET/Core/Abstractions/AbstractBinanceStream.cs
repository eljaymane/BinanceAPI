using BinanceAPI.NET.Core.Interfaces;
using BinanceAPI.NET.Core.Models;
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
        }

        internal void CreateDataStore()
        {
            if (!Client.StreamData.ContainsKey(StreamType)) Client.StreamData.TryAdd(StreamType, new Dictionary<string, IBinanceStreamData>());
        }
    }
}
