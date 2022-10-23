using e_shop_api.Core.Dto;

namespace EShop.Logic.Applications.Product.Query
{
    public class QueryProductResponse : BaseResponse
    {
        public CommonDto.Product Product { get; set; }
    }
}