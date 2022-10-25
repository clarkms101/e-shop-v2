using MediatR;

namespace EShop.Logic.Applications.Admin.Command.Logout
{
    public class LogoutRequest : IRequest<LogoutResponse>
    {
        public string ApiAccessKey { get; set; }
    }
}