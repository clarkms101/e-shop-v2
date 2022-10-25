using e_shop_api.Core.Dto;

namespace EShop.Logic.Applications.Products.Query;

public class EsQueryProductsResponse : BaseResponse
{
    public List<Product.CommonDto.Product> Products { get; set; }
    public Pagination Pagination { get; set; }
}