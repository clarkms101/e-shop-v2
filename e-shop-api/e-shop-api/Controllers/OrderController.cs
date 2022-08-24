using System.Threading.Tasks;
using e_shop_api.Applications.Order.Command.Create;
using e_shop_api.Applications.Order.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace e_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:int}")]
        public async Task<QueryOrderResponse> Get([FromRoute] int id)
        {
            var request = new QueryOrderRequest()
            {
                OrderId = id
            };
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpPost]
        public async Task<CreateOrderResponse> Post([FromBody] CreateOrderRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }
    }
}