using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Core.Const;
using EasyNetQ;
using EasyNetQ.Topology;
using EShop.Logic.Applications.Order.Command.Update;
using EShop.MQ.Producer;

namespace e_shop_api.MQConsumer
{
    public class MqConsumer
    {
        private readonly CancelOrderHandler _cancelOrderHandler;
        private readonly IAdvancedBus _advancedBus;
        private const string ExchangeName = "e-shop.direct";

        public MqConsumer(IBus bus, CancelOrderHandler cancelOrderHandler)
        {
            _cancelOrderHandler = cancelOrderHandler;
            _advancedBus = bus.Advanced;
        }

        public async Task DoWork()
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

                await _cancelOrderHandler.Handle(new CancelOrderRequest()
                {
                    OrderId = orderId
                }, CancellationToken.None);
            });
        }
    }
}