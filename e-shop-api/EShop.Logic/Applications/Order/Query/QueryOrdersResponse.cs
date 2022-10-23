using e_shop_api.Core.Dto;
using EShop.Logic.Applications.Order.CommonDto;

namespace EShop.Logic.Applications.Order.Query
{
    public class QueryOrdersResponse : BaseResponse
    {
        public List<OrderInfo> OrderInfos { get; set; }
        public Pagination Pagination { get; set; }
    }
}