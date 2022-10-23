using System.Collections.Generic;
using e_shop_api.Applications.Order.CommonDto;
using e_shop_api.Core.Dto;

namespace e_shop_api.Applications.Order.Query
{
    public class QueryOrdersResponse : BaseResponse
    {
        public List<OrderInfo> OrderInfos { get; set; }
        public Pagination Pagination { get; set; }
    }
}