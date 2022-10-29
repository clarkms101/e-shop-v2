using System.Text;
using e_shop_api.Core.Const;
using e_shop_api.Core.Dto.MQ;
using e_shop_api.Core.Dto.Product;
using e_shop_api.Core.Enumeration;
using EasyNetQ;
using EasyNetQ.Topology;
using Newtonsoft.Json;

namespace EShop.MQ.Producer
{
    public class MqProducer
    {
        private readonly IAdvancedBus _advancedBus;
        private const string DelayExchangeName = "e-shop.direct.delay";
        private const string NormalExchangeName = "e-shop.direct";

        public MqProducer(IBus bus)
        {
            _advancedBus = bus.Advanced;
        }

        /// <summary>
        /// 同步商品資訊到ES
        /// </summary>
        /// <param name="syncType"></param>
        /// <param name="product"></param>
        public async Task SyncEsProductData(DateSyncType syncType, EsProduct product)
        {
            const string functionName = "sync-product";
            const string routingKey = RoutingKey.SyncProductKey;

            var exchange = await _advancedBus.ExchangeDeclareAsync(NormalExchangeName, ExchangeType.Direct);

            var queue = await _advancedBus.QueueDeclareAsync(
                $"{functionName}");

            await _advancedBus.BindAsync(exchange, queue, routingKey, null);

            var data = new EsProductSyncInfo()
            {
                SyncType = syncType,
                EsProduct = product
            };
            var message = JsonConvert.SerializeObject(data);
            var body = Encoding.UTF8.GetBytes(message);
            await _advancedBus.PublishAsync(exchange, routingKey, false, new MessageProperties { DeliveryMode = 2 },
                body);
        }

        /// <summary>
        /// 將訂單加入自動取消檢查排程
        /// </summary>
        public async Task SetOrderAutoCancelSchedule(int orderId)
        {
            const string functionName = "order-auto-cancel";
            const string routingKey = RoutingKey.OrderAutoCancelKey;

            var exchange =
                await _advancedBus.ExchangeDeclareAsync(DelayExchangeName,
                    cfg => cfg.AsDelayedExchange(ExchangeType.Direct));

            var queue = await _advancedBus.QueueDeclareAsync(
                $"{functionName}");

            await _advancedBus.BindAsync(exchange, queue, routingKey, null);

            // publish 訊息，DeliveryMode 是用來設定 message persist (1:non-persistent / 2:persistent)
            var msgHeaders = new MessageProperties
            {
                DeliveryMode = 2
            };
            // 設定5分鐘後執行 5 * 60 * 1000 => 300000
            // 單位:毫秒
            msgHeaders.Headers.Add("x-delay", 300000);
            var message = orderId.ToString();
            var body = Encoding.UTF8.GetBytes(message);
            await _advancedBus.PublishAsync(exchange, routingKey, false, msgHeaders, body);
        }
    }
}