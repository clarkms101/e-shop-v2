using e_shop_api.Enumeration;
using MediatR;

namespace e_shop_api.Applications.Order.Command.Update
{
    public class UpdateOrderRequest : IRequest<UpdateOrderResponse>
    {
        public string SerialNumber { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}