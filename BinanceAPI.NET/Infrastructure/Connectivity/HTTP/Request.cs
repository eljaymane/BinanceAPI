using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Connectivity.HTTP
{
    public class Request
    {
        private HttpRequestMessage request;
        private HttpClient client;

        public int RequestId;

        public Request(HttpRequestMessage request, HttpClient client,int requestId)
        {
            this.request = request;
            this.client = client;
            RequestId = requestId;
        }
    }
}
