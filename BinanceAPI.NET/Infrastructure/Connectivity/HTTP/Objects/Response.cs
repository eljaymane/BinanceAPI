using System.Net;
using BinanceAPI.NET.Infrastructure.Interfaces;

namespace BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Objects
{
    public class Response : IResponse
    {
        public HttpResponseMessage response;

        public HttpStatusCode StatusCode => response.StatusCode;
        public bool IsSuccess => response.IsSuccessStatusCode;

        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers => response.Headers;

        public Response(HttpResponseMessage message)
        {
            response = message;
        }

        public async Task<Stream> GetResponseStreamAsync()
        {
            return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            response.Dispose();
        }
    }
}