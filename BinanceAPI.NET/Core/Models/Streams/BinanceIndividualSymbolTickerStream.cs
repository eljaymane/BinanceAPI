using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Socket;
using BinanceAPI.NET.Infrastructure.Abstractions;
using BinanceAPI.NET.Infrastructure.Connectivity.Socket;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Streams
{
    public class BinanceIndividualSymbolTickerStream 
    {
        public BinanceIndividualSymbolTickerStream(string symbol, CancellationTokenSource tokenSource,ILoggerFactory loggerFactory) 
        {
        }

        //public override void Initialize()
        //{
        //    throw new NotImplementedException();
        //}

        //public override void OnClose()
        //{
        //    throw new NotImplementedException();
        //}

        //public override void OnError(Exception exception)
        //{
        //    throw new NotImplementedException();
        //}

        //public override void OnMessage(string message)
        //{
        //    throw new NotImplementedException();
        //}

        //public override void OnOpen()
        //{
        //    throw new NotImplementedException();
        //}

        //public override void OnReconnected()
        //{
        //    throw new NotImplementedException();
        //}

        //public override void OnReconnecting()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
