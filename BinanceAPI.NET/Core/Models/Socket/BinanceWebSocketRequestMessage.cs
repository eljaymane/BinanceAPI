using BinanceAPI.NET.Core.Models.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BinanceAPI.NET.Core.Models.Socket
{
    [Serializable]
    internal class BinanceWebSocketRequestMessage
    {
        [JsonPropertyName("id")]
        private uint Id;
        [JsonPropertyName("method")]
        private BinanceRequestMessageType MessageType;
        [JsonPropertyName("params")]
        private string[] Parameters;
        public BinanceWebSocketRequestMessage(uint id,BinanceRequestMessageType messageType, string[] parameters) 
        {
            Id = id;
            MessageType = messageType;
            Parameters = parameters;
        }
    }
}
