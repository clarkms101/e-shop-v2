using EShop.Cache.Interface;
using EShop.Logic.Applications.Admin.Command.LoginCheck;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.Admin.Command.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutRequest, LogoutResponse>
    {
        private readonly IAdminInfoCacheUtility _adminInfoCacheUtility;
        private readonly ILogger<LoginCheckHandler> _logger;

        public LogoutHandler(IAdminInfoCacheUtility adminInfoCacheUtility, ILogger<LoginCheckHandler> logger)
        {
            _adminInfoCacheUtility = adminInfoCacheUtility;
            _logger = logger;
        }

        public async Task<LogoutResponse> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            _adminInfoCacheUtility.RemoveAdminInfo(request.ApiAccessKey);

            return new LogoutResponse()
            {
                Success = true,
                Message = "已登出"
            };
        }
    }
}