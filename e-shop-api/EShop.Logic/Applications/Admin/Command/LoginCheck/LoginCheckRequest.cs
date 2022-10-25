using MediatR;

namespace EShop.Logic.Applications.Admin.Command.LoginCheck
{
    public class LoginCheckRequest : IRequest<LoginCheckResponse>
    {
        public string ApiAccessKey { get; set; }
    }
}