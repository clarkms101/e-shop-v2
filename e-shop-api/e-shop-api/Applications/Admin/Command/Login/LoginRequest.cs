using MediatR;

namespace e_shop_api.Applications.Admin.Command.Login
{
    public class LoginRequest : IRequest<LoginResponse>
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}