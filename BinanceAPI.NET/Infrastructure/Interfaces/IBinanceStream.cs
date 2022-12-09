namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    public interface IBinanceStream
    {
        void SubscribeAsync();

        void OnPingMessage();
    }
}