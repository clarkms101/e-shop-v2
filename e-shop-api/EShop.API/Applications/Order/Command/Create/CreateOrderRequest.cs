using e_shop_api.Applications.Order.CommonDto;
using e_shop_api.Core.Dto;
using MediatR;

namespace e_shop_api.Applications.Order.Command.Create
{
    public class CreateOrderRequest : BaseCommandRequest, IRequest<CreateOrderResponse>
    {
        public OrderForm OrderForm { get; set; }
    }
}