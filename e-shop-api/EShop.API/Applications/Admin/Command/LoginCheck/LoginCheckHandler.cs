using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Utility.Dto;
using e_shop_api.Utility.Interface;
using EShop.Cache.Interface;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace e_shop_api.Applications.Admin.Command.LoginCheck
{
    public class LoginCheckHandler : IRequestHandler<LoginCheckRequest, LoginCheckResponse>
    {
        private readonly IMemoryCacheUtility _memoryCacheUtility;
        private readonly ILogger<LoginCheckHandler> _logger;

        public LoginCheckHandler(IMemoryCacheUtility memoryCacheUtility, ILogger<LoginCheckHandler> logger)
        {
            _memoryCacheUtility = memoryCacheUtility;
            _logger = logger;
        }

        public async Task<LoginCheckResponse> Handle(LoginCheckRequest request, CancellationToken cancellationToken)
        {
            var adminInfoJsonString = _memoryCacheUtility.Get<string>(request.ApiAccessKey);
            if (string.IsNullOrWhiteSpace(adminInfoJsonString) == false)
            {
                var adminCacheInfo = JsonConvert.DeserializeObject<AdminInfo>(adminInfoJsonString);
                if (adminCacheInfo != null)
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