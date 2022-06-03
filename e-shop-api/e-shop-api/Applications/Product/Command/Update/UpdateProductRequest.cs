using MediatR;

namespace e_shop_api.Applications.Product.Command.Update
{
    public class UpdateProductRequest : IRequest<UpdateProductResponse>
    {
        public CommonDto.Product Product { get; set; }
    }
}