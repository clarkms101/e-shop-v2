using System.Threading.Tasks;
using e_shop_api.Applications.Admin.Command.Login;
using e_shop_api.Applications.Admin.Command.LoginCheck;
using e_shop_api.Applications.Admin.Command.Logout;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace e_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<JsonResult> Login([FromBody] LoginRequest request)
        {
            var result = await _mediator.Send(request);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> Logout([FromBody] LogoutRequest request)
        {
            var result = await _mediator.Send(request);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> LoginCheck([FromBody] LoginCheckRequest request)
        {
            var result = await _mediator.Send(request);
            return new JsonResult(result);
        }
    }
}