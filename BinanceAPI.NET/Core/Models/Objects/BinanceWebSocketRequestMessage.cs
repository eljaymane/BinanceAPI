using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Core.Models.Enums;
using BinanceAPI.NET.Core.Models.Socket.Clients;
using BinanceAPI.NET.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BinanceAPI.NET.Core.Models.Objects
{
    [Serializable]
    public class BinanceWebSocketRequestMessage : IBinanceRequest, IRequestDataType
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("method"), JsonConverter(typeof(RequestMessageTypeJsonConverter))]
        public BinanceRequestMessageType MessageType { get; set; }
        [JsonProperty("params")]
        public string[] Parameters { get; set; }

        public BinanceWebSocketRequestMessage()
        {

        }
        public BinanceWebSocketRequestMessage(uint id, BinanceRequestMessageType messageType, string[] parameters)
        {
            Id = id;
            MessageType = messageType;
            Parameters = parameters;
        }
    }
}
