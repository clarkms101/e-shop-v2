using System.Collections.Generic;
using e_shop_api.Core.Dto;

namespace e_shop_api.Applications.Cart.Query
{
    public class QueryCartResponse : BaseResponse
    {
        public List<CommonDto.Cart> Carts { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FinalTotalAmount { get; set; }
    }
}