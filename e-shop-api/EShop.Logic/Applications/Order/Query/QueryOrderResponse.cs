using e_shop_api.Core.Dto;
using EShop.Logic.Applications.Order.CommonDto;

namespace EShop.Logic.Applications.Order.Query
{
    public class QueryOrderResponse : BaseResponse
    {
        public OrderInfo OrderInfo { get; set; }
    }
}