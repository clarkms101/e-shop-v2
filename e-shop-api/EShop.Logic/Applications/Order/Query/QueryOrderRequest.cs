using MediatR;

namespace EShop.Logic.Applications.Order.Query
{
    public class QueryOrderRequest : IRequest<QueryOrderResponse>
    {
        public string SerialNumber { get; set; }
    }
}