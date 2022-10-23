using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Applications.Admin.Command.LoginCheck;
using EShop.Cache.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Admin.Command.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutRequest, LogoutResponse>
    {
        private readonly IMemoryCacheUtility _memoryCacheUtility;
        private readonly ILogger<LoginCheckHandler> _logger;

        public LogoutHandler(IMemoryCacheUtility memoryCacheUtility, ILogger<LoginCheckHandler> logger)
        {
            _memoryCacheUtility = memoryCacheUtility;
            _logger = logger;
        }

        public async Task<LogoutResponse> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            _memoryCacheUtility.Remove(request.ApiAccessKey);

            return new LogoutResponse()
            {
                Success = true,
                Message = "已登出"
            };
        }
    }
}