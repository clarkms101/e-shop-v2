using MediatR;

namespace EShop.Logic.Applications.Admin.Command.Login
{
    public class LoginRequest : IRequest<LoginResponse>
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}