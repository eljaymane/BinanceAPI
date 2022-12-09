using BinanceAPI.NET.Core.Converters;
using BinanceAPI.NET.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Models.Enums
{
    [Serializable]
    [JsonConverter(typeof(RequestMessageTypeJsonConverter))]
    internal enum BinanceRequestMessageType
    {
        [StringValue("SUBSCRIBE")]
        Subscribe,
        [StringValue("UNSUBSCRIBE")]
        Unsubscribe,
        [StringValue("LIST_SUBSCRIPTIONS")]
        ListSubscriptions,
        [StringValue("SET_PROPERTY")]
        SetProperty,
        [StringValue("GET_PROPERTY")]
        GetProperty,
        [StringValue("GET_PROPERTY_BY_NAME")]
        GET_PROPERTY_BY_NAME,
    }
}
