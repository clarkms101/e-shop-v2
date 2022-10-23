using e_shop_api.Core.Dto;
using MediatR;

namespace EShop.Logic.Applications.Product.Command.Create
{
    public class CreateProductRequest : BaseCommandRequest, IRequest<CreateProductResponse>
    {
        public CommonDto.Product Product { get; set; }
    }
}