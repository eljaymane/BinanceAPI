namespace BinanceAPI.NET.Core.Interfaces
{
    public interface IBinanceStream
    {
        void SubscribeAsync();

        void OnPingMessage();
    }
}