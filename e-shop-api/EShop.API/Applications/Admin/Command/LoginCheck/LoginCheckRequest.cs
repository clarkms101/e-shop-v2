using MediatR;

namespace e_shop_api.Applications.Admin.Command.LoginCheck
{
    public class LoginCheckRequest : IRequest<LoginCheckResponse>
    {
        public string ApiAccessKey { get; set; }
    }
}