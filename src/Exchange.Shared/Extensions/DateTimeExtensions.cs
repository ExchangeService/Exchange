using System;

namespace Exchange.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime AsDateTime(this decimal unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return epoch.AddSeconds((long)unixTime);
        }

        public static int AsDaysSinceEpoch(this DateTime dateTime) => (dateTime - default(DateTime)).Days;
    }
}