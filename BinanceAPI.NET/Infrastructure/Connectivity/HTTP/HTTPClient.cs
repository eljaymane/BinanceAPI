using BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Connectivity.HTTP
{
    public class HTTPClient : IHTTPClient
    {
        public bool IsAuthenticated = false;
        private HttpConfiguration Configuration { get; set; }
        public HTTPClient(HttpConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task Authenticate()
        {
            using(var client = new HttpClient()) 
            {
                var message = new HttpRequestMessage(HttpMethod.Post, Configuration.BaseUri);
                foreach(var header in Configuration.Headers)
                    message.Headers.Add(header.Key, header.Value);
                foreach(var param in Configuration.Parameters)
                    message.Properties.Add(param.Key, param.Value);

                await client.SendAsync(message).ConfigureAwait(false);

            }
        }

    }
}
