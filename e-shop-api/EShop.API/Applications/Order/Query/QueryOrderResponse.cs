using e_shop_api.Applications.Order.CommonDto;
using e_shop_api.Core.Dto;

namespace e_shop_api.Applications.Order.Query
{
    public class QueryOrderResponse : BaseResponse
    {
        public OrderInfo OrderInfo { get; set; }
    }
}