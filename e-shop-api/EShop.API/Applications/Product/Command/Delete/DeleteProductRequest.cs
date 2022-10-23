using e_shop_api.Core.Dto;
using MediatR;

namespace e_shop_api.Applications.Product.Command.Delete
{
    public class DeleteProductRequest : BaseCommandRequest, IRequest<DeleteProductResponse>
    {
        public int ProductId { get; set; }
    }
}