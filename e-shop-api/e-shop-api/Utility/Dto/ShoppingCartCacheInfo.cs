using System.Collections.Generic;

namespace e_shop_api.Utility.Dto
{
    public class ShoppingCartCacheInfo
    {
        /// <summary>
        /// 購物車編號 ps: 使用者帳號
        /// </summary>
        public string CartId { get; set; }
        public int? CouponId { get; set; }
        public List<ShoppingCartItemCache> ShoppingCartItems { get; set; }
    }
}