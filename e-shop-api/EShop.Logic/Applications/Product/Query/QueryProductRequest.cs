using MediatR;

namespace EShop.Logic.Applications.Product.Query
{
    public class QueryProductRequest : IRequest<QueryProductResponse>
    {
        public int ProductId { get; set; }
    }
}