using System.Threading.Tasks;
using e_shop_api.ActionFilters;
using EShop.Logic.Applications.Order.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace e_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AdminAuthActionFilter]
        [HttpPost]
        public async Task<QueryOrdersResponse> Post([FromBody] QueryOrdersRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }
    }
}