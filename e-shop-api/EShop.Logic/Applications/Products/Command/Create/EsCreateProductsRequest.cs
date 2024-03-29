using e_shop_api.Core.Dto;
using e_shop_api.Core.Dto.Product;
using MediatR;

namespace EShop.Logic.Applications.Products.Command.Create;

public class EsCreateProductsRequest : BaseCommandRequest, IRequest<EsCreateProductsResponse>
{
    public List<EsProduct> Products { get; set; }
}