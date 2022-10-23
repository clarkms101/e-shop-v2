using e_shop_api.Core.Dto;
using MediatR;

namespace EShop.Logic.Applications.Coupon.Query
{
    public class QueryCouponsRequest : BaseQueryPageRequest, IRequest<QueryCouponsResponse>
    {
    }
}