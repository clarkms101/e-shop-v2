using e_shop_api.Core.Dto;

namespace EShop.Logic.Applications.Coupon.Query
{
    public class QueryCouponsResponse : BaseResponse
    {
        public List<CommonDto.Coupon> Coupons { get; set; }
        public Pagination Pagination { get; set; }
    }
}