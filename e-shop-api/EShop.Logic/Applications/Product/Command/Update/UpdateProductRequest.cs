using e_shop_api.Core.Dto;
using MediatR;

namespace EShop.Logic.Applications.Product.Command.Update
{
    public class UpdateProductRequest : BaseCommandRequest, IRequest<UpdateProductResponse>
    {
        public CommonDto.Product Product { get; set; }
    }
}