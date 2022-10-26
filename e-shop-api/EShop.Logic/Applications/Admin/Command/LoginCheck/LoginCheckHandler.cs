using e_shop_api.Core.Utility.Dto;
using EShop.Cache.Interface;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EShop.Logic.Applications.Admin.Command.LoginCheck
{
    public class LoginCheckHandler : IRequestHandler<LoginCheckRequest, LoginCheckResponse>
    {
        private readonly IAdminInfoCacheUtility _adminInfoCacheUtility;
        private readonly ILogger<LoginCheckHandler> _logger;

        public LoginCheckHandler(IAdminInfoCacheUtility adminInfoCacheUtility, ILogger<LoginCheckHandler> logger)
        {
            _adminInfoCacheUtility = adminInfoCacheUtility;
            _logger = logger;
        }

        public async Task<LoginCheckResponse> Handle(LoginCheckRequest request, CancellationToken cancellationToken)
        {
            var adminCacheInfo = _adminInfoCacheUtility.GetAdminInfo(request.ApiAccessKey);
            if (adminCacheInfo != null)
            {
                return new LoginCheckResponse()
                {
                    Success = true,
                    Message = "Online",
                    Account = adminCacheInfo.Account,
                    ExpiredTimeStamp = adminCacheInfo.ExpiredTimeStamp
                };
            }

            return new LoginCheckResponse()
            {
                Success = false,
                Message = "Offline"
            };
        }
    }
}