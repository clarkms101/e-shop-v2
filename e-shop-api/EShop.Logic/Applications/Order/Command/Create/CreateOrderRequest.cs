using e_shop_api.Core.Dto;
using EShop.Logic.Applications.Order.CommonDto;
using MediatR;

namespace EShop.Logic.Applications.Order.Command.Create
{
    public class CreateOrderRequest : BaseCommandRequest, IRequest<CreateOrderResponse>
    {
        public OrderForm OrderForm { get; set; }
    }
}