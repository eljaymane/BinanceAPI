namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    public interface IAuthenticationProvider
    {
        IRequest AuthenticateRequest(IRequest request);
    }
}