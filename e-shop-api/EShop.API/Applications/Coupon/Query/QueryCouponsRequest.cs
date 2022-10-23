using e_shop_api.Core.Dto;
using MediatR;

namespace e_shop_api.Applications.Coupon.Query
{
    public class QueryCouponsRequest : BaseQueryPageRequest, IRequest<QueryCouponsResponse>
    {
    }
}