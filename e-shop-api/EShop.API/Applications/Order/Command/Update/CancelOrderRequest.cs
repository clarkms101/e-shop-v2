using e_shop_api.Core.Dto;
using MediatR;

namespace e_shop_api.Applications.Order.Command.Update
{
    public class CancelOrderRequest : BaseCommandRequest, IRequest<CancelOrderResponse>
    {
        public int OrderId { get; set; }
    }
}