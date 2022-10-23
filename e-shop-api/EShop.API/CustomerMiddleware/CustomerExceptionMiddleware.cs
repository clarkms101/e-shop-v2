using System;
using System.Net;
using System.Threading.Tasks;
using e_shop_api.Applications;
using e_shop_api.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace e_shop_api.CustomerMiddleware
{
    public class CustomerExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CustomerExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<CustomerExceptionMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            _logger.LogError(ex, $"例外錯誤訊息: {ex.ToString()}");

            var result = JsonConvert.SerializeObject(new BaseResponse()
            {
                Success = false,
                Message = "處理失敗!"
            });
            return context.Response.WriteAsync(result);
        }
    }
}