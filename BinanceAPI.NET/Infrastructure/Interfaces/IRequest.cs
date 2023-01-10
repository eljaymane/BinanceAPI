namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    /// <summary>
    /// Contract of the request to be sent using the websocket client.
    /// </summary>
    public interface IRequest : IDisposable
    {
        IRequest AddHeader(KeyValuePair<string, string> header);
        IRequest AddStringContent(string content, string contentType);
        IRequest AddByteContent(byte[] content);
    }
}