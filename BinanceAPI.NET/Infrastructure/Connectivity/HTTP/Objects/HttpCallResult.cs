using BinanceAPI.NET.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Objects
{
    public class HttpCallResult
    {
        public bool Success => Response.IsSuccess;

        public IResponse Response { get; }

        public HttpCallResult()
        {

        }
    }

    public class HttpCallResult<T> : HttpCallResult
    {
        public T? Data { get; set; }

        public HttpCallResult([AllowNull]T data)
        {
            Data = data;
        }
    }
}
