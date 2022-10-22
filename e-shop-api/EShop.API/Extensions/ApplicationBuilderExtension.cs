using e_shop_api.Applications.Order.Command.Update;
using e_shop_api.CustomerMiddleware;
using e_shop_api.RMQ;
using EasyNetQ;
using EShop.Entity.DataBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseCustomerExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomerExceptionMiddleware>();
        }

        public static IApplicationBuilder MessageQueueSubscribe(this IApplicationBuilder appBuilder)
        {
            var services = appBuilder.ApplicationServices.CreateScope().ServiceProvider;

            var lifeTime = services.GetService<IHostApplicationLifetime>();
            if (lifeTime == null) return appBuilder;

            var context = services.GetRequiredService<EShopDbContext>();
            var log = services.GetRequiredService<ILogger<CancelOrderHandler>>();
            var bus = services.GetService<IBus>();

            lifeTime.ApplicationStarted.Register(callback: Callback);
            lifeTime.ApplicationStopped.Register(() => bus?.Dispose());

            async void Callback()
            {
                var mqConsumer = new MqConsumer(bus, new CancelOrderHandler(context, log));
                await mqConsumer.DoWork();
            }

            return appBuilder;
        }
    }
}