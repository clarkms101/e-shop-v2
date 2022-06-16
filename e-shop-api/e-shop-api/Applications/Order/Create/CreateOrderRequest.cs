using e_shop_api.Applications.Order.CommonDto;
using MediatR;

namespace e_shop_api.Applications.Order.Create
{
    public class CreateOrderRequest : IRequest<CreateOrderResponse>
    {
        public OrderForm OrderForm { get; set; }
    }
}