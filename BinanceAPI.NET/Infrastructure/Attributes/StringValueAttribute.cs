using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Attributes
{
    /// <summary>
    /// This attribute makes it possible to have a string value attached to an enum by using [StringValue("")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class StringValueAttribute : Attribute
    {
        public string Value { get; set; }

        public StringValueAttribute(string value)
        {
            Value = value;
        }


    }
}
