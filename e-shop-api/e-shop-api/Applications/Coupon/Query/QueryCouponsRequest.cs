using MediatR;

namespace e_shop_api.Applications.Coupon.Query
{
    public class QueryCouponsRequest : IRequest<QueryCouponsResponse>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}