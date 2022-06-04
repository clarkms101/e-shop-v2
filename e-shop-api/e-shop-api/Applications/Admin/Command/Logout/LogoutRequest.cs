using MediatR;

namespace e_shop_api.Applications.Admin.Command.Logout
{
    public class LogoutRequest : IRequest<LogoutResponse>
    {
        public string ApiAccessKey { get; set; }
    }
}