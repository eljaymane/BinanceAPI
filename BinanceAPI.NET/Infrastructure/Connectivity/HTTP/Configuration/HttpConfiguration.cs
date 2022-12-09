using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Objects;

namespace BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Configuration
{
    public class HttpConfiguration
    {
        public string BaseUri { get; set; }

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public List<KeyValuePair<string, string>> Headers { get; set; } = new();
        public List<KeyValuePair<string, string>> Parameters { get; set; } = new();

        public ApiCredentials Credentials { get; set; }
        public TimeSpan Timeout { get; set; }

        public HttpConfiguration(ApiCredentials apiCredentials)
        {
            Credentials = apiCredentials;
            Headers.Add(new KeyValuePair<string, string>("X-MBX-APIKEY", ""));

        }

        
    }
}
