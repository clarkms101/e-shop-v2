using e_shop_api.CustomerMiddleware;
using e_shop_api.MQConsumer;
using EasyNetQ;
using EShop.Entity.DataBase;
using EShop.Logic.Applications.Order.Command.Update;
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
            var provider = appBuilder.ApplicationServices.CreateScope().ServiceProvider;

            var lifeTime = provider.GetService<IHostApplicationLifetime>();
            if (lifeTime == null) return appBuilder;

            var factory = provider.GetRequiredService<IServiceScopeFactory>();
            var log = provider.GetRequiredService<ILogger<CancelOrderHandler>>();
            var bus = provider.GetService<IBus>();

            lifeTime.ApplicationStarted.Register(callback: Callback);
            lifeTime.ApplicationStopped.Register(() => bus?.Dispose());

            async void Callback()
            {
                var mqConsumer = new MqConsumer(bus, new CancelOrderHandler(factory, log));
                await mqConsumer.OrderAutoCancel();
            }

            return appBuilder;
        }
    }
}