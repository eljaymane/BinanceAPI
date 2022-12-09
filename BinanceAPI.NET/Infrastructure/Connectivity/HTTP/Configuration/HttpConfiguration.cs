using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Configuration
{
    public class HttpConfiguration
    {
        public string BaseUri { get; set; }

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public List<KeyValuePair<string, string>> Headers { get; set; } = new();
        public List<KeyValuePair<string, string>> Parameters { get; set; } = new();

        public ApiCredentials Credentials { get; set; }

        public HttpConfiguration(ApiCredentials apiCredentials)
        {
            Credentials = apiCredentials;
            Headers.Add(new KeyValuePair<string, string>("X-MBX-APIKEY", ""));

        }

        public string GetHmacSha256Signature(string key)
        {
            byte[] keyBytes= Encoding.GetBytes(key);
            byte[] toEncodeBytes = Encoding.GetBytes(Credentials.ApiSecret);

            byte[] hash;
            using(HMACSHA256 encoder = new(keyBytes))
                hash = encoder.ComputeHash(toEncodeBytes);

            return BitConverter.ToString(hash).Replace("-","");
        }
    }
}
