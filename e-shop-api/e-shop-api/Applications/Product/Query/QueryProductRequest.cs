using MediatR;

namespace e_shop_api.Applications.Product.Query
{
    public class QueryProductRequest : IRequest<QueryProductResponse>
    {
        public int ProductId { get; set; }
    }
}