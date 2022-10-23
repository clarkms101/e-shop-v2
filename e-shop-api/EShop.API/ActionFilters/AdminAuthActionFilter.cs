using e_shop_api.Core.Utility.Interface;
using e_shop_api.Utility;
using EShop.Cache.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace e_shop_api.ActionFilters
{
    public class AdminAuthActionFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 基本驗證檢查
            if (context.HttpContext.User == null ||
                !context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();

                return;
            }

            // 檢查請求有無JWT
            var jwtUtility = context.HttpContext.RequestServices.GetRequiredService<IJwtUtility>();
            var adminInfo = jwtUtility.GetAdminInfo();
            if (string.IsNullOrWhiteSpace(adminInfo.ApiAccessKey))
            {
                Unauthorized(ref context);
            }

            // 檢查請求的JWT是否存在於登入快取清單中
            var memoryCacheUtility = context.HttpContext.RequestServices.GetRequiredService<IMemoryCacheUtility>();
            var adminInfoJsonString = memoryCacheUtility.Get<string>(adminInfo.ApiAccessKey);
            if (string.IsNullOrWhiteSpace(adminInfoJsonString))
            {
                Unauthorized(ref context);
            }
        }

        private static void Unauthorized(ref AuthorizationFilterContext context)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}