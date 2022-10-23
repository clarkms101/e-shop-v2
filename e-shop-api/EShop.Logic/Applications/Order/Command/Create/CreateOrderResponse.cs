using e_shop_api.Core.Dto;

namespace EShop.Logic.Applications.Order.Command.Create
{
    public class CreateOrderResponse : BaseResponse
    {
        public string SerialNumber { get; set; }
    }
}