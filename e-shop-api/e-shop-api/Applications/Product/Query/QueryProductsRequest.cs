using MediatR;

namespace e_shop_api.Applications.Product.Query
{
    public class QueryProductsRequest : IRequest<QueryProductsResponse>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
    }
}