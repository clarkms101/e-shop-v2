using e_shop_api.CustomerMiddleware;
using Microsoft.AspNetCore.Builder;

namespace e_shop_api.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseCustomerExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomerExceptionMiddleware>();
        }
    }
}