using System.Threading.Tasks;
using e_shop_api.ActionFilters;
using EShop.Logic.Applications.Admin.Command.Login;
using EShop.Logic.Applications.Admin.Command.LoginCheck;
using EShop.Logic.Applications.Admin.Command.Logout;
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
        public async Task<LoginResponse> Login([FromBody] LoginRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [AdminAuthActionFilter]
        [HttpPost]
        public async Task<LogoutResponse> Logout([FromBody] LogoutRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpPost]
        public async Task<LoginCheckResponse> LoginCheck([FromBody] LoginCheckRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }
    }
}