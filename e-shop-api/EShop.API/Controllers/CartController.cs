using System.Threading.Tasks;
using e_shop_api.Applications.Cart.Command.Create;
using e_shop_api.Applications.Cart.Command.Delete;
using e_shop_api.Applications.Cart.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace e_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<QueryCartResponse> Get([FromQuery] QueryCartRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpPost]
        public async Task<CreateCartDetailResponse> Post([FromBody] CreateCartDetailRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<DeleteCartDetailResponse> Delete([FromRoute] string id)
        {
            var request = new DeleteCartDetailRequest()
            {
                CartDetailId = id
            };
            var result = await _mediator.Send(request);
            return result;
        }
    }
}