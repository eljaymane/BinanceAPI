using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinanceAPI.NET.Infrastructure.Interfaces;

namespace BinanceAPI.NET.Infrastructure.Connectivity.Socket.Configuration
{
    /// <summary>
    /// The configuration object used by the socket client.
    /// </summary>
    public class SocketConfiguration : ISocketConfiguration
    {
        public int SOCKET_BUFFER_SIZE { get { return 65536; } }
        public Uri BaseUri { get; set; }
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string,string>();
        public IDictionary<string, string> Cookies { get; set; } = new Dictionary<string, string>();
        public TimeSpan ReconnectInterval { get; set; } = TimeSpan.FromSeconds(5);
        public bool AutoReconnect { get; set; }
        public TimeSpan? Timeout { get; set; }
        public TimeSpan? KeepAliveInterval { get; set; }
        public int? PacketPerSecond { get; set; } = 5;
        public string? Origin { get; set; }

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public SocketConfiguration(Uri baseUri,bool autoReconnect)
        {
            BaseUri = baseUri;
            AutoReconnect = autoReconnect;
        }
    }
}
