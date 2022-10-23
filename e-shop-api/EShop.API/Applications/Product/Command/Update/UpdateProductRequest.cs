using e_shop_api.Core.Dto;
using MediatR;

namespace e_shop_api.Applications.Product.Command.Update
{
    public class UpdateProductRequest : BaseCommandRequest, IRequest<UpdateProductResponse>
    {
        public CommonDto.Product Product { get; set; }
    }
}