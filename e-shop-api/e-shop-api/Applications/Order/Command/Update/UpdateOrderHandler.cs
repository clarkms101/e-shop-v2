using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.DataBase;
using e_shop_api.Enumeration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Order.Command.Update
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderRequest, UpdateOrderResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<UpdateOrderHandler> _logger;

        public UpdateOrderHandler(EShopDbContext eShopDbContext,
            ILogger<UpdateOrderHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
        }

        public async Task<UpdateOrderResponse> Handle(UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var order = await _eShopDbContext.Orders.Where(s => s.SerialNumber == request.SerialNumber)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            var message = "處理中";
            if (order != null)
            {
                switch (request.OrderStatus)
                {
                    case OrderStatus.Finished:
                        order.IsPaid = true;
                        order.OrderStatus = OrderStatus.Finished.ToString();
                        message = "訂單支付已完成";
                        break;
                    case OrderStatus.Cancel:
                        order.OrderStatus = OrderStatus.Cancel.ToString();
                        message = "訂單已取消";
                        break;
                    case OrderStatus.Refund:
                        order.OrderStatus = OrderStatus.Refund.ToString();
                        message = "訂單已退款";
                        break;
                }
                order.LastModificationTime = DateTime.Now;

                await _eShopDbContext.SaveChangesAsync(cancellationToken);

                return new UpdateOrderResponse()
                {
                    Success = true,
                    Message = message
                };
            }
            else
            {
                return new UpdateOrderResponse()
                {
                    Success = false,
                    Message = "查無此訂單!"
                };
            }
        }
    }
}