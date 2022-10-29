using e_shop_api.Core.Dto;
using e_shop_api.Core.Dto.Product;
using MediatR;

namespace EShop.Logic.Applications.Product.Command.Create;

public class EsCreateProductRequest : BaseCommandRequest, IRequest<EsCreateProductResponse>
{
    public EsProduct Product { get; set; }
}