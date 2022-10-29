using e_shop_api.Core.Dto;
using e_shop_api.Core.Dto.Product;
using MediatR;

namespace EShop.Logic.Applications.Product.Command.Update;

public class EsUpdateProductRequest : BaseCommandRequest, IRequest<EsUpdateProductResponse>
{
    public EsProduct Product { get; set; }
}