using System.Runtime.Caching;
using e_shop_api.Core.Extensions;
using e_shop_api.Core.Utility.Dto;
using EShop.Cache.Interface;
using EShop.Entity.DataBase;
using EShop.Logic.Utility;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EShop.Logic.Applications.Admin.Command.Login
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly IJwtUtility _jwtUtility;
        private readonly IAdminInfoCacheUtility _adminInfoCacheUtility;
        private readonly ILogger<LoginHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginHandler(EShopDbContext eShopDbContext, IJwtUtility jwtUtility,
            IAdminInfoCacheUtility adminInfoCacheUtility, ILogger<LoginHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _eShopDbContext = eShopDbContext;
            _jwtUtility = jwtUtility;
            _adminInfoCacheUtility = adminInfoCacheUtility;
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
                _adminInfoCacheUtility.RemoveAdminInfo(apiAccessKey);

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
                    _adminInfoCacheUtility.AddAdminInfo(apiAccessKey, adminCacheInfo);

                    _logger.LogInformation($"登入成功! : adminInfo: {JsonConvert.SerializeObject(adminCacheInfo)}");

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