using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Socket;
using BinanceAPI.NET.Infrastructure.Extensions;
using BinanceAPI.NET.Infrastructure.Interfaces;

namespace BinanceAPI.NET.Infrastructure.Abstractions
{
    public class AbstractBinanceStream : IBinanceStream
    {
        private string Symbol;
        public string? Name;
        private BinanceStreamType StreamType;

        public IWebSocket Client;
        public CancellationTokenSource TokenSource;


        public AbstractBinanceStream(string symbol, BinanceStreamType streamType, CancellationTokenSource tokenSource)
        {
            Name = symbol.ToLower() + StreamType.GetStringValue();
            Symbol = symbol;
            StreamType = streamType;
            TokenSource = tokenSource;
        }

  

        public void OnPingMessage()
        {
           // Client.OnPingMessage(TokenSource.Token);
        }

        public void SubscribeAsync()
        {
        }
    }
}
