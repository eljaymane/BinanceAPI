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
        private AuthenticationProvider authenticationProvider;
        public IRequestFactory RequestFactory { get; set; }

        public abstract TimeSyncInfo GetTimeSyncInfo();
        public abstract TimeSpan GetTimeOffset();
        public int TotalRequestsMade { get; set; } = 0;
        private HttpConfiguration Configuration { get; set; }
        public AbstractRestApiClient(ILogger logger, HttpConfiguration configuration)
        {
            Configuration = configuration;
            RequestFactory.Configure(configuration.Timeout,Configuration);
        }

        //protected virtual IRequest ConstructRequest(string apiRoute,HttpMethod method,HttpParametersPosition parametersPosition, int requestId,bool timeSync,Dictionary<string,object>? parameters, Dictionary<string, string>? additionalHeaders)
        //{
            
        //    return new Request()
        //    //In body parameters
        //}
        //protected virtual async Task<HttpCallResult> PrepareRequestAsync()
        //{
        //    return Task.FromResult<HttpCallResult>(new HttpCallResult());
        //}

        private int NextId()
        {
            return TotalRequestsMade + 1;
        }

        protected virtual async Task SendRequestAsync()
        {

        }



    }
}
