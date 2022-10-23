using e_shop_api.Core.Dto;

namespace e_shop_api.Applications.Order.Command.Create
{
    public class CreateOrderResponse : BaseResponse
    {
        public string SerialNumber { get; set; }
    }
}