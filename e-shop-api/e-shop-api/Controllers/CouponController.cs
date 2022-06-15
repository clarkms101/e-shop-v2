using System.Threading.Tasks;
using e_shop_api.ActionFilters;
using e_shop_api.Applications.Coupon.Command.Create;
using e_shop_api.Applications.Coupon.Command.Delete;
using e_shop_api.Applications.Coupon.Command.Update;
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

        [AdminAuthActionFilter]
        [HttpDelete("{id:int}")]
        public async Task<JsonResult> Delete([FromRoute] int id)
        {
            var request = new DeleteCouponRequest()
            {
                CouponId = id
            };
            var result = await _mediator.Send(request);
            return new JsonResult(result);
        }

        [AdminAuthActionFilter]
        [HttpPut]
        public async Task<JsonResult> Put([FromBody] UpdateCouponRequest request)
        {
            var result = await _mediator.Send(request);
            return new JsonResult(result);
        }
    }
}