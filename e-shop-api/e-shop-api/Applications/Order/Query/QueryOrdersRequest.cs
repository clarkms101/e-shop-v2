using MediatR;

namespace e_shop_api.Applications.Order.Query
{
    public class QueryOrdersRequest : IRequest<QueryOrdersResponse>
    {
        public int Page { get; set; }
        public int PaymentMethod { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}