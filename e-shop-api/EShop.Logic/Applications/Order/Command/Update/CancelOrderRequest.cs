using e_shop_api.Core.Dto;
using MediatR;

namespace EShop.Logic.Applications.Order.Command.Update
{
    public class CancelOrderRequest : BaseCommandRequest, IRequest<CancelOrderResponse>
    {
        public int OrderId { get; set; }
    }
}