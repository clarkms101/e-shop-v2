using e_shop_api.Core.Dto;
using EShop.Logic.Applications.Products.Dto;

namespace EShop.Logic.Applications.Products.Query;

public class EsQueryProductsResponse : BaseResponse
{
    public List<ProductDto> Products { get; set; }
    public Pagination Pagination { get; set; }
}