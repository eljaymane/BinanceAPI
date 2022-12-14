using System.Text;

namespace BinanceAPI.NET.Infrastructure.Interfaces
{
    public interface ISocketConfiguration
    {
        public int SOCKET_BUFFER_SIZE { get; }
        public Uri BaseUri { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public IDictionary<string, string> Cookies { get; set; }
        public TimeSpan ReconnectInterval { get; set; }
        public bool AutoReconnect { get; set; }
        public TimeSpan? Timeout { get; set; }
        public TimeSpan? KeepAliveInterval { get; set; }
        public int? PacketPerSecond { get; set; }
        public Encoding Encoding { get; set; }
    }
}