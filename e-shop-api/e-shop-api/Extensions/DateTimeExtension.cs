using System;

namespace e_shop_api.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime)
        {
            // 統一轉成 UTC+0
            return DateTime.SpecifyKind(dateTime.ToUniversalTime(), DateTimeKind.Utc);
        }

        public static long ToTimeStamp(this DateTime dateTime)
        {
            // 統一轉成 UTC+0
            return dateTime.ToDateTimeOffset().ToUnixTimeSeconds();
        }
    }
}