using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceBOT
{
    public class GlobalConfig 
    {
        public string ApiKey { get { return _config["ApiKey"]; } }
        public string SecretKey { get { return _config["SecretKey"]; } }
        public string? this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        private IConfiguration _config { get; }
        public int SOCKET_BUFFER_SIZE { get { return 1024; } }

        public GlobalConfig(IConfiguration config)
        {
            _config = config;
        }


        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }
    }
}
