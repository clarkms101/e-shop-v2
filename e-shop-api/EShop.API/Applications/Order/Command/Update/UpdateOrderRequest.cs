using e_shop_api.Core.Dto;
using e_shop_api.Core.Enumeration;
using MediatR;

namespace e_shop_api.Applications.Order.Command.Update
{
    public class UpdateOrderRequest : BaseCommandRequest, IRequest<UpdateOrderResponse>
    {
        public string SerialNumber { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}