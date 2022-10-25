using e_shop_api.Core.Dto.Cart;

namespace EShop.Logic.Applications.Cart.CommonDto
{
    public class Cart
    {
        public string CartDetailId { get; set; }
        public ShoppingCoupon? Coupon { get; set; }
        public ShoppingProduct Product { get; set; }
        public int Qty { get; set; }
    }
}