using System.Threading.Tasks;
using e_shop_api.Applications.Coupon.Query;
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
        public async Task<JsonResult> Get([FromQuery] QueryCouponsRequest request)
        {
            var result = await _mediator.Send(request);
            return new JsonResult(result);
        }
    }
}