using e_shop_api.CustomerMiddleware;
using e_shop_api.MQConsumer;
using EasyNetQ;
using EShop.Logic.Applications.Order.Command.Update;
using EShop.Logic.Applications.Product.Command.Create;
using EShop.Logic.Applications.Product.Command.Delete;
using EShop.Logic.Applications.Product.Command.Update;
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
            var bus = provider.GetService<IBus>();

            lifeTime.ApplicationStarted.Register(callback: Callback);
            lifeTime.ApplicationStopped.Register(() => bus?.Dispose());

            async void Callback()
            {
                var mqConsumer = new MqConsumer(bus);

                await mqConsumer.OrderAutoCancel(new CancelOrderHandler(factory,
                    provider.GetRequiredService<ILogger<CancelOrderHandler>>()));

                await mqConsumer.SyncEsProductData(
                    new EsCreateProductHandler(factory, provider.GetRequiredService<ILogger<EsCreateProductHandler>>()),
                    new EsDeleteProductHandler(factory, provider.GetRequiredService<ILogger<EsDeleteProductHandler>>()),
                    new EsUpdateProductHandler(factory, provider.GetRequiredService<ILogger<EsUpdateProductHandler>>())
                );
            }

            return appBuilder;
        }
    }
}