using System;
using System.Threading.Tasks;
using e_shop_api.ActionFilters;
using EShop.Logic.Applications.Order.Command.Create;
using EShop.Logic.Applications.Order.Command.Update;
using EShop.Logic.Applications.Order.Query;
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

        [HttpGet("{serialNumber:guid}")]
        public async Task<QueryOrderResponse> Get([FromRoute] Guid serialNumber)
        {
            var request = new QueryOrderRequest()
            {
                SerialNumber = serialNumber.ToString()
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

        [AdminAuthActionFilter]
        [HttpPut]
        public async Task<UpdateOrderResponse> Put([FromBody] UpdateOrderRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }
    }
}