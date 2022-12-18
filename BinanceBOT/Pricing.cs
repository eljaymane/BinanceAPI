using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BinanceBOT
{
    public class Pricing
    {
        public static decimal RoundDecimal(decimal price)
        {
            return Math.Round(price, 2,MidpointRounding.ToZero);
        }

        public static decimal GetPrice(decimal Quantity,decimal Price)
        {
            return Math.Round(decimal.Multiply(Quantity, Price),3,MidpointRounding.ToZero);
        }

       public static decimal MaxBuyQuantity(decimal Price,decimal Available)
        {
            return Math.Round(decimal.Divide(Available,Price),4,MidpointRounding.ToZero);
        }
    }
}
