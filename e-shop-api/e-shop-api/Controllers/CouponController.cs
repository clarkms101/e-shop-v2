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
        public async Task<CreateCouponResponse> Post([FromBody] CreateCouponRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [AdminAuthActionFilter]
        [HttpDelete("{id:int}")]
        public async Task<DeleteCouponResponse> Delete([FromRoute] int id)
        {
            var request = new DeleteCouponRequest()
            {
                CouponId = id
            };
            var result = await _mediator.Send(request);
            return result;
        }

        [AdminAuthActionFilter]
        [HttpPut]
        public async Task<UpdateCouponResponse> Put([FromBody] UpdateCouponRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }
    }
}