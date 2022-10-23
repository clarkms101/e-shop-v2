using e_shop_api.Core.Dto;
using MediatR;

namespace EShop.Logic.Applications.Order.Query
{
    public class QueryOrdersRequest : BaseQueryPageRequest, IRequest<QueryOrdersResponse>
    {
        public int PaymentMethod { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}