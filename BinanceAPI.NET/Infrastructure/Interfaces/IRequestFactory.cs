using BinanceAPI.NET.Infrastructure.Connectivity.HTTP;
using BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Objects;

namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    public interface IRequestFactory
    {
        IRequestFactory Configure(TimeSpan timeout, HttpClient? httpClient = null);
        IRequest CreateRequest(HttpMethod method, Uri uri, int requestId);
    }
}