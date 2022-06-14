using System.Threading.Tasks;
using e_shop_api.ActionFilters;
using e_shop_api.Applications.Coupon.Command.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace e_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CouponController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AdminAuthActionFilter]
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] CreateCouponRequest request)
        {
            var result = await _mediator.Send(request);
            return new JsonResult(result);
        }
    }
}