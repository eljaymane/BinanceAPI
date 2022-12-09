using BinanceAPI.NET.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Extensions
{
    public static class StringValueExtension
    {
        public static string? GetStringValue(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo? fieldInfo = type.GetField(value.ToString());

            StringValueAttribute[]? attributes = fieldInfo?.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            return attributes?.Length > 0 ? attributes[0].Value : null;

        }
    }
}
