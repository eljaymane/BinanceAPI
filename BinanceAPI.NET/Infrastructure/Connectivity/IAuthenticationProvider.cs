using BinanceAPI.NET.Infrastructure.Interfaces;

namespace BinanceAPI.NET.Infrastructure.Connectivity
{
    public interface IAuthenticationProvider
    {
        IRequest AuthenticateRequest(IRequest request);
    }
}