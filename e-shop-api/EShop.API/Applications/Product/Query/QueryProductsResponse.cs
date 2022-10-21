using System.Collections.Generic;

namespace e_shop_api.Applications.Product.Query
{
    public class QueryProductsResponse : BaseResponse
    {
        public List<CommonDto.Product> Products { get; set; }
        public Pagination Pagination { get; set; }
    }
}