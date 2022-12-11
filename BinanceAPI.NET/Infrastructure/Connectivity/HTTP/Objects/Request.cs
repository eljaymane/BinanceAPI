using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BinanceAPI.NET.Infrastructure.Interfaces;

namespace BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Objects
{
    public class Request : IRequest
    {
        private HttpRequestMessage request;
        private HttpClient client;

        public int RequestId;
        public string Content { get; private set; }

        public Request(HttpRequestMessage request, HttpClient client, int requestId)
        {
            this.request = request;
            this.client = client;
            RequestId = requestId;
        }

        public string Accept
        {
            set => request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(value));
        }

        public HttpMethod Method
        {
            get => request.Method;
            set => request.Method = value;
        }

        public Uri Uri => request.RequestUri;

        public IRequest AddHeader(KeyValuePair<string, string> header)
        {
            request.Headers.Add(header.Key, header.Value);
            return this;
        }

        public IRequest AddStringContent(string content, string contentType)
        {
            Content = content;
            request.Content = new StringContent(content, Encoding.UTF8, contentType);
            return this;
        }

        public IRequest AddByteContent(byte[] content)
        {
            request.Content = new ByteArrayContent(content);
            return this;
        }

        public Dictionary<string, IEnumerable<string>> GetHeaders()
        {
            return request.Headers.ToDictionary(h => h.Key, h => h.Value);
        }

        public async Task<IResponse> GetResponseAsync(CancellationToken cancellationToken)
        {
            return new Response(await client.SendAsync(request, cancellationToken).ConfigureAwait(false));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
