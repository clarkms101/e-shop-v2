using System.Threading.Tasks;
using EShop.Logic.Applications.Coupon.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace e_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CouponsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<QueryCouponsResponse> Get([FromQuery] QueryCouponsRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }
    }
}