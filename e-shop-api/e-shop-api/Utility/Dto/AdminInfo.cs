namespace e_shop_api.Utility.Dto
{
    public class AdminInfo
    {
        public string ApiAccessKey { get; set; }
        public string SystemUserId { get; set; }
        public string Account { get; set; }
        public string Permission { get; set; }
        public string Device { get; set; }
        public long ExpiredTimeStamp { get; set; }
    }
}