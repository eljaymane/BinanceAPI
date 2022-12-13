using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Core.Helpers
{
    public static class Calculation
    {
        public static DateTime EpochToDate(long epoch)
        {
            //DateTime baseEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0,DateTimeKind.Utc);
            return DateTimeOffset.FromUnixTimeMilliseconds(epoch).DateTime;
        }

        public static long DateToEpoch(DateTime date)
        {
            return long.Parse((TimeZoneInfo.ConvertTimeToUtc(date) - new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc)).TotalMilliseconds.ToString());
        }
    }
}
