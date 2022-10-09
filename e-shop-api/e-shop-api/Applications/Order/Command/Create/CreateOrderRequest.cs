using e_shop_api.Applications.Order.CommonDto;
using MediatR;

namespace e_shop_api.Applications.Order.Command.Create
{
    public class CreateOrderRequest : BaseCommandRequest, IRequest<CreateOrderResponse>
    {
        public OrderForm OrderForm { get; set; }
    }
}