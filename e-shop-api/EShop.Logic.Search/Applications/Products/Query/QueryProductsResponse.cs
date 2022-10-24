using e_shop_api.Core.Dto;
using EShop.Logic.Search.Applications.Products.Dto;

namespace EShop.Logic.Search.Applications.Products.Query
{
    public class QueryProductsResponse : BaseResponse
    {
        public List<ProductDto> Products { get; set; }
        public Pagination Pagination { get; set; }
    }
}