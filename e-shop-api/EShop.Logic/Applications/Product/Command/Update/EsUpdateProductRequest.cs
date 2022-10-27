using e_shop_api.Core.Dto;
using EShop.Logic.Applications.Product.CommonDto;
using MediatR;

namespace EShop.Logic.Applications.Product.Command.Update;

public class EsUpdateProductRequest : BaseCommandRequest, IRequest<EsUpdateProductResponse>
{
    public EsProduct Product { get; set; }
}