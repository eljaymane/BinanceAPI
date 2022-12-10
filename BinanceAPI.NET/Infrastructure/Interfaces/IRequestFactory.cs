using BinanceAPI.NET.Infrastructure.Abstractions;
using BinanceAPI.NET.Infrastructure.Connectivity.HTTP;
using BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Configuration;
using BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Objects;
using BinanceAPI.NET.Infrastructure.Enums;

namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    public interface IRequestFactory
    {
        IRequestFactory Configure(TimeSpan timeout, HttpConfiguration configuration, HttpClient? httpClient = null);
        IRequest CreateRequest(string uri, HttpMethod method, HttpParametersPosition parametersPosition, int requestId, bool timeSync,
            AuthenticationProvider? authenticationProvider, Dictionary<string, object>? parameters, Dictionary<string, string>? additionalHeaders);
    }
}