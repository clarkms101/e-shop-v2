using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.DataBase;
using e_shop_api.Extensions;
using e_shop_api.Utility.Dto;
using e_shop_api.Utility.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace e_shop_api.Applications.Admin.Command.Login
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly IJwtUtility _jwtUtility;
        private readonly IMemoryCacheUtility _memoryCacheUtility;
        private readonly ILogger<LoginHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginHandler(EShopDbContext eShopDbContext, IJwtUtility jwtUtility,
            IMemoryCacheUtility memoryCacheUtility, ILogger<LoginHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _eShopDbContext = eShopDbContext;
            _jwtUtility = jwtUtility;
            _memoryCacheUtility = memoryCacheUtility;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var adminInfo = _eShopDbContext.Admins
                .SingleOrDefault(s => s.Account == request.Account && s.Password == request.Password.Md5Encrypt());

            if (adminInfo != null)
            {
                var apiAccessKey = $"Admin-{adminInfo.Account}";

                // 先刪除原本的登入資訊
                _memoryCacheUtility.Remove(apiAccessKey);

                // 產生Token和到期時間
                if (_httpContextAccessor.HttpContext != null)
                {
                    var device = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
                    var adminCacheInfo = new AdminInfo()
                    {
                        Account = adminInfo.Account,
                        SystemUserId = adminInfo.Id.ToString(),
                        ApiAccessKey = apiAccessKey,
                        Permission = adminInfo.Permission.ToString(),
                        Device = device,
                        ExpiredTimeStamp = _jwtUtility.GetExpiredTimeStamp()
                    };

                    // 存入登入資訊到快取(供登入狀態確認和登出處理)
                    // todo 排程定期檢查到期 AccessKey 並刪除
                    var adminCacheInfoJsonString = JsonConvert.SerializeObject(adminCacheInfo);
                    _memoryCacheUtility.Add(new CacheItem(apiAccessKey, adminCacheInfoJsonString),
                        new CacheItemPolicy());

                    _logger.LogInformation($"登入成功! : adminInfo: {adminCacheInfoJsonString}");

                    return new LoginResponse()
                    {
                        Success = true,
                        Message = "登入成功!",
                        Token = _jwtUtility.GenerateAdminToken(adminCacheInfo),
                        ExpiredTimeStamp = adminCacheInfo.ExpiredTimeStamp
                    };
                }
            }

            return new LoginResponse()
            {
                Success = false,
                Message = "帳密錯誤!"
            };
        }
    }
}