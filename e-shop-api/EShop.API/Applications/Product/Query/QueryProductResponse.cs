using e_shop_api.Core.Dto;

namespace e_shop_api.Applications.Product.Query
{
    public class QueryProductResponse : BaseResponse
    {
        public CommonDto.Product Product { get; set; }
    }
}