using MediatR;

namespace e_shop_api.Applications.Product.Command.Create
{
    public class CreateProductRequest : IRequest<CreateProductResponse>
    {
        public CommonDto.Product Product { get; set; }
    }
}