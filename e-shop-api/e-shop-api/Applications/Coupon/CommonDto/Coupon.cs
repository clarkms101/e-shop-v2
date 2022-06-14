namespace e_shop_api.Applications.Coupon.CommonDto
{
    public class Coupon
    {
        public int CouponId { get; set; }
        public string Title { get; set; }
        public string CouponCode { get; set; }

        /// <summary>
        /// 90 => 9折
        /// </summary>
        public int Percent { get; set; }

        public bool IsEnabled { get; set; }
        public long DueDateTimeStamp { get; set; }
    }
}