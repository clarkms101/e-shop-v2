using System.Collections.Generic;

namespace e_shop_api.Applications.Coupon.Query
{
    public class QueryCouponsResponse : BaseResponse
    {
        public List<CommonDto.Coupon> Coupons { get; set; }
        public Pagination Pagination { get; set; }
    }
}