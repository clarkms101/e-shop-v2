using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Core.Const;
using e_shop_api.Core.Dto.MQ;
using e_shop_api.Core.Enumeration;
using EasyNetQ;
using EasyNetQ.Topology;
using EShop.Logic.Applications.Order.Command.Update;
using EShop.Logic.Applications.Product.Command.Create;
using EShop.Logic.Applications.Product.Command.Delete;
using EShop.Logic.Applications.Product.Command.Update;
using Newtonsoft.Json;

namespace e_shop_api.MQConsumer
{
    public class MqConsumer
    {
        private readonly IAdvancedBus _advancedBus;
        private const string ExchangeName = "e-shop.direct";

        public MqConsumer(IBus bus)
        {
            _advancedBus = bus.Advanced;
        }

        // todo 快取更新機制 (資料來源:新增、更新、刪除時)

        /// <summary>
        /// 同步商品資訊到ES
        /// </summary>
        public async Task SyncEsProductData(
            EsCreateProductHandler esCreateProductHandler,
            EsDeleteProductHandler esDeleteProductHandler,
            EsUpdateProductHandler esUpdateProductHandler
        )
        {
            const string functionName = "sync-product";
            const string routingKey = RoutingKey.SyncProductKey;

            var exchange = await _advancedBus.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct);

            var queue = await _advancedBus.QueueDeclareAsync(
                $"{functionName}");

            await _advancedBus.BindAsync(exchange, queue, routingKey, null);

            _advancedBus.Consume(queue, async (body, properties, info) =>
            {
                var message = Encoding.UTF8.GetString(body.ToArray());

                Console.WriteLine($"Got message: '{message}',{DateTime.Now}");

                var data = JsonConvert.DeserializeObject<EsProductSyncInfo>(message);

                if (data != null && data.EsProduct != null)
                {
                    switch (data.SyncType)
                    {
                        case DateSyncType.Create:
                            await esCreateProductHandler.Handle(new EsCreateProductRequest()
                            {
                                SystemUserId = 0,
                                Product = data.EsProduct
                            }, CancellationToken.None);
                            break;
                        case DateSyncType.Update:
                            await esUpdateProductHandler.Handle(new EsUpdateProductRequest()
                            {
                                SystemUserId = 0,
                                Product = data.EsProduct
                            }, CancellationToken.None);
                            break;
                        case DateSyncType.Delete:
                            await esDeleteProductHandler.Handle(new EsDeleteProductRequest()
                            {
                                SystemUserId = 0,
                                ProductId = data.EsProduct.Id
                            }, CancellationToken.None);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            });
        }

        /// <summary>
        /// 取消訂單排程
        /// </summary>
        public async Task OrderAutoCancel(CancelOrderHandler cancelOrderHandler)
        {
            const string functionName = "order-auto-cancel";
            const string routingKey = RoutingKey.OrderAutoCancelKey;

            var exchange = await _advancedBus.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct);

            var queue = await _advancedBus.QueueDeclareAsync(
                $"{functionName}");

            await _advancedBus.BindAsync(exchange, queue, routingKey, null);

            _advancedBus.Consume(queue, async (body, properties, info) =>
            {
                var message = Encoding.UTF8.GetString(body.ToArray());

                Console.WriteLine($"Got message: '{message}',{DateTime.Now}");

                var orderId = Convert.ToInt32(message);

                await cancelOrderHandler.Handle(new CancelOrderRequest()
                {
                    OrderId = orderId
                }, CancellationToken.None);
            });
        }
    }
}