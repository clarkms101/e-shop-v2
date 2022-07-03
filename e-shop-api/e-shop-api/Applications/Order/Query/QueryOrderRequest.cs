using MediatR;

namespace e_shop_api.Applications.Order.Query
{
    public class QueryOrderRequest : IRequest<QueryOrderResponse>
    {
        public int OrderId { get; set; }
    }
}