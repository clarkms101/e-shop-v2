using MediatR;

namespace e_shop_api.Applications.Product.Command.Delete
{
    public class DeleteProductRequest : IRequest<DeleteProductResponse>
    {
        public int ProductId { get; set; }
    }
}