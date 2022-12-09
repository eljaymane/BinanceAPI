namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    public interface IRequest : IDisposable
    {
        IRequest AddHeader(KeyValuePair<string, string> header);
        IRequest AddStringContent(string content, string contentType);
        IRequest AddByteContent(byte[] content);
    }
}