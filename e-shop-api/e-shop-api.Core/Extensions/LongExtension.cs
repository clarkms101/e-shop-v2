namespace e_shop_api.Core.Extensions
{
    public static class LongExtension
    {
        public static DateTime ToDateTime(this long timeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}