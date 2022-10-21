namespace e_shop_api.Applications.Cart.CommonDto
{
    public class ShoppingCoupon
    {
        public int CouponId { get; set; }
        public string Title { get; set; }
        public string CouponCode { get; set; }
        public int Percent { get; set; }
        public bool IsEnabled { get; set; }
        public long DueDateTimeStamp { get; set; }
    }
}