using System.Collections.Generic;

namespace e_shop_api.Applications.Cart.Query
{
    public class QueryCartResponse : BaseResponse
    {
        public List<CommonDto.Cart> Carts { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FinalTotalAmount { get; set; }
    }
}