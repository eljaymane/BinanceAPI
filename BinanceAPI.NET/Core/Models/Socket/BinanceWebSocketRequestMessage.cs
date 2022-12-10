using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Socket.Clients;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BinanceAPI.NET.Core.Models.Socket
{
    [Serializable]
    public class BinanceWebSocketRequestMessage : IBinanceRequest
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }
        [JsonPropertyName("method"), JsonConverter(typeof(RequestMessageTypeJsonConverter))]
        public BinanceRequestMessageType MessageType { get; set; }
        [JsonPropertyName("params")]
        public string[] Parameters { get; set; }

        public BinanceWebSocketRequestMessage()
        {

        }
        public BinanceWebSocketRequestMessage(uint id,BinanceRequestMessageType messageType, string[] parameters) 
        {
            Id = id;
            MessageType = messageType;
            Parameters = parameters;
        }
    }
}
