using System.Threading.Tasks;
using e_shop_api.Applications.Cart.Command.Update;
using e_shop_api.Applications.Order.Command.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace e_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShoppingController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public ShoppingController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<CreditCardPayOrderResponse> CreditCardPay([FromBody] CreditCardPayOrderRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }
        
        [HttpPost]
        public async Task<UpdateCartCouponResponse> UseCoupon([FromBody] UpdateCartCouponRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }
    }
}