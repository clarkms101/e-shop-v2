using e_shop_api.Core.Dto;

namespace EShop.Logic.Applications.Product.Query
{
    public class QueryProductsResponse : BaseResponse
    {
        public List<CommonDto.Product> Products { get; set; }
        public Pagination Pagination { get; set; }
    }
}