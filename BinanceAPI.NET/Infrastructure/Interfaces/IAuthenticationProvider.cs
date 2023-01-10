namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    /// <summary>
    /// Authentication provider contract that describes a class that is able to authenticate any IRequest.
    /// </summary>
    public interface IAuthenticationProvider
    {
        IRequest AuthenticateRequest(IRequest request);
    }
}