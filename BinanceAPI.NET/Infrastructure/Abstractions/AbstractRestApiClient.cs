using BinanceAPI.NET.Infrastructure.Connectivity;
using BinanceAPI.NET.Infrastructure.Connectivity.HTTP;
using BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Configuration;
using BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Objects;
using BinanceAPI.NET.Infrastructure.Enums;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Abstractions
{
    public abstract class AbstractRestApiClient : IRestClient
    {
        public ILogger Logger;
        private AbstractAuthenticationProvider authenticationProvider;
        public IRequestFactory RequestFactory { get; set; } = new RequestFactory();

        public abstract TimeSyncInfo GetTimeSyncInfo();
        public abstract TimeSpan GetTimeOffset();
        public int TotalRequestsMade { get; set; } = 0;
        private HttpConfiguration Configuration { get; set; }
        public AbstractRestApiClient(ILogger logger, HttpConfiguration configuration)
        {
            Configuration = configuration;
            RequestFactory.Configure(configuration.Timeout);
        }

        protected virtual IRequest ConstructRequest(string apiRoute,HttpMethod method,HttpParametersPosition parametersPosition, int requestId,bool timeSync,Dictionary<string,object>? parameters, Dictionary<string, string>? additionalHeaders)
        {
            parameters ??= new();
            additionalHeaders??= new();
            var absoluteUri = String.Concat(Configuration.BaseUri, apiRoute);
            absoluteUri = parametersPosition == HttpParametersPosition.InUri ? absoluteUri + "?" : absoluteUri;
            for (int i = 0; i < parameters.Count; i++)   
            {
                if (parametersPosition == HttpParametersPosition.InUri) 
                    absoluteUri += i == parameters.Count - 1 ? 
                        $"{parameters.ElementAt(i).Key}={parameters.ElementAt(i).Value}" : 
                        $"{parameters.ElementAt(i).Key}={parameters.ElementAt(i).Value}&";
            }
            Uri uri = new Uri(absoluteUri);
            var request = (Request)RequestFactory.CreateRequest(method, uri, NextId());
            request.Accept = Constants.JsonContentHeader;
            if (authenticationProvider != null) request = authenticationProvider.AuthenticateRequest(request);

            foreach (var header in additionalHeaders)
            {
                request.AddHeader(new KeyValuePair<string, string>(header.Key, header.Value));
            }

            foreach (var header in Configuration.Headers) //Default global headers
            {
                request.AddHeader(new KeyValuePair<string, string>(header.Key, header.Value));
            }
        }
        protected virtual async Task<HttpCallResult> PrepareRequestAsync()
        {

        }

        private int NextId()
        {
            return TotalRequestsMade + 1;
        }

        protected virtual async Task SendRequestAsync()
        {

        }



    }
}
