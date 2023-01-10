using BinanceAPI.NET.Infrastructure.Abstractions;
using BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Configuration;
using BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Objects;
using BinanceAPI.NET.Infrastructure.Enums;
using BinanceAPI.NET.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Connectivity.HTTP
{
    /// <summary>
    /// A factory that can correctly generate an IRequest, add a content, header to it. If provided, an AuthenticationProvider is used to authenticate the request.
    /// </summary>
    public class RequestFactory : IRequestFactory
    {
        private HttpClient? _httpClient;
        private HttpConfiguration? Configuration;
        public RequestFactory()
        {

        }

        public IRequestFactory Configure(TimeSpan timeout,HttpConfiguration configuration,HttpClient? httpClient = null)
        {
            Configuration = configuration;
            if(httpClient == null)
            {
                HttpMessageHandler messageHandler = new HttpClientHandler();
                _httpClient = new HttpClient(messageHandler) { Timeout = timeout };
            } else
            {
                _httpClient = httpClient;
            }
            return this;
        }

        public IRequest CreateRequest(string uri, HttpMethod method, HttpParametersPosition parametersPosition, int requestId, bool timeSync,
            AuthenticationProvider? authenticationProvider, Dictionary<string, object>? parameters, Dictionary<string, string>? additionalHeaders)
        {
            if (Configuration == null) return null;
            HttpRequestMessage message;
            parameters ??= new();
            additionalHeaders ??= new();
            var absoluteUri = parametersPosition == HttpParametersPosition.InUri ? uri + "?" : uri;
            if (parametersPosition == HttpParametersPosition.InUri)
                for (int i = 0; i < parameters.Count; i++)
            {
               
                    absoluteUri += i == parameters.Count - 1 ?
                        $"{parameters.ElementAt(i).Key}={parameters.ElementAt(i).Value}" :
                        $"{parameters.ElementAt(i).Key}={parameters.ElementAt(i).Value}&";
            }

            message = new(method,new Uri(absoluteUri));
            var request = new Request(message, _httpClient!, requestId);
            request.Accept = Constants.JsonContentHeader;
            if(authenticationProvider != null)
                request = (Request?)authenticationProvider?.AuthenticateRequest(request);

            foreach (var header in additionalHeaders)
            {
                request!.AddHeader(new KeyValuePair<string, string>(header.Key, header.Value));
            }

            foreach (var header in Configuration.Headers) //Default global headers
            {
                request!.AddHeader(new KeyValuePair<string, string>(header.Key, header.Value));
            }

            //In Body parameters
            return request!;
        }
    }
}
