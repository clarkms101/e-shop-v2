using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using e_shop_api.Config;
using e_shop_api.Extensions;
using e_shop_api.Utility.Dto;
using e_shop_api.Utility.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace e_shop_api.Utility
{
    public class JwtUtility : IJwtUtility
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string JwtKeyApiAccessKey = "JwtKeyApiAccessKey";
        private const string JwtKeyAdminAccount = "JwtKeyAdminAccount";
        private const string JwtKeyAdminPermission = "JwtKeyAdminPermission";

        public JwtUtility(IOptions<JwtConfig> jwtConfig, IHttpContextAccessor httpContextAccessor)
        {
            _jwtConfig = jwtConfig.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public long GetExpiredTimeStamp()
        {
            return DateTime.Now.AddMinutes(60).ToTimeStamp();
        }

        /// <summary>
        /// 將使用者資訊放到JWT裡面,並產生Token
        /// </summary>
        /// <param name="adminInfo"></param>
        /// <returns></returns>
        public string GenerateAdminToken(AdminInfo adminInfo)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                // todo
                var device = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
            }

            var claims = new List<Claim>
            {
                // 登入角色身份
                new Claim(ClaimTypes.Role, "Admin"),
                // 將使用者資訊加入到JWT Token
                new Claim(JwtKeyApiAccessKey, adminInfo.ApiAccessKey),
                new Claim(JwtKeyAdminAccount, adminInfo.Account),
                new Claim(JwtKeyAdminPermission, adminInfo.Permission),
            };

            var token = new JwtSecurityToken(
                // Header & Signature
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_jwtConfig.SignKey)),
                        SecurityAlgorithms.HmacSha256)
                ),
                // Payload
                new JwtPayload(
                    _jwtConfig.Issuer,
                    _jwtConfig.Audience,
                    claims,
                    DateTime.UtcNow,
                    // 到期時間 jwt
                    DateTime.UtcNow.AddMinutes(_jwtConfig.Expires))
            );

            return $"Bearer {new JwtSecurityTokenHandler().WriteToken(token)}";
        }

        /// <summary>
        /// 從JWT裡面解析出使用者資訊
        /// </summary>
        /// <returns></returns>
        public AdminInfo GetAdminInfo()
        {
            return new AdminInfo()
            {
                ApiAccessKey = GetJwtClaimValue(JwtKeyApiAccessKey),
                Account = GetJwtClaimValue(JwtKeyAdminAccount),
                Permission = GetJwtClaimValue(JwtKeyAdminPermission)
            };
        }

        /// <summary>
        /// 取得JWT保存於快取的Key
        /// </summary>
        /// <param name="apiAccessKey"></param>
        /// <returns></returns>
        public string GetApiAccessKey(long apiAccessKey)
        {
            return $"EShop.{apiAccessKey}";
        }

        private string GetJwtClaimValue(string key)
        {
            return _httpContextAccessor.HttpContext != null
                ? _httpContextAccessor.HttpContext.User.GetClaimValue(key)
                : string.Empty;
        }
    }

    internal static class UserClaim
    {
        internal static string GetClaimValue(this ClaimsPrincipal claimsPrincipal, string key)
        {
            return claimsPrincipal.Claims.First(b => b.Type == key).Value;
        }
    }
}