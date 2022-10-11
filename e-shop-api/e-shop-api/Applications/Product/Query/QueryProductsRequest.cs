using MediatR;

namespace e_shop_api.Applications.Product.Query
{
    public class QueryProductsRequest : BaseQueryPageRequest, IRequest<QueryProductsResponse>
    {
        public int? CategoryId { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
    }
}