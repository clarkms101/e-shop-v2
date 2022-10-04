using MediatR;

namespace e_shop_api.Applications.Order.Query
{
    public class QueryOrderRequest : IRequest<QueryOrderResponse>
    {
        public string SerialNumber { get; set; }
    }
}