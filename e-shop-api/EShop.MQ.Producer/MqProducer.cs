using System.Text;
using EasyNetQ;
using EasyNetQ.Topology;

namespace EShop.MQ.Producer
{
    public class MqProducer
    {
        private readonly IAdvancedBus _advancedBus;
        private const string ExchangeName = "e-shop.direct.delay";

        public MqProducer(IBus bus)
        {
            _advancedBus = bus.Advanced;
        }

        /// <summary>
        /// 將訂單加入自動取消檢查排程
        /// </summary>
        public async Task SetOrderAutoCancelSchedule(int orderId)
        {
            const string functionName = "order-auto-cancel";
            const string routingKey = "order-auto-cancel-key";

            var exchange =
                await _advancedBus.ExchangeDeclareAsync(ExchangeName,
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