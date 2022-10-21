using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Core.Enumeration;
using e_shop_api.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Order.Command.Update
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderRequest, CancelOrderResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<CancelOrderHandler> _logger;

        public CancelOrderHandler(EShopDbContext eShopDbContext,
            ILogger<CancelOrderHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
        }

        public async Task<CancelOrderResponse> Handle(CancelOrderRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"訂單編號:${request.OrderId} - 訂單自動取消檢查!");

            var order = await _eShopDbContext.Orders.Where(s => s.Id == request.OrderId)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);

            // ============ 不需取消訂單
            if (order.IsPaid)
            {
                const string message = "已完成支付，不需取消";
                _logger.LogDebug($"訂單編號:${request.OrderId} - {message}");
                return new CancelOrderResponse()
                {
                    Success = false,
                    Message = message
                };
            }

            // ============ 需取消訂單
            // status
            order.OrderStatus = OrderStatus.Cancel.ToString();
            // system
            order.LastModifierUserId = 0; // 系統編號
            order.LastModificationTime = DateTime.Now;

            await _eShopDbContext.SaveChangesAsync(cancellationToken);

            const string messageCancel = "自動取消成功!";
            _logger.LogDebug($"訂單編號:${request.OrderId} - {messageCancel}");
            return new CancelOrderResponse()
            {
                Success = true,
                Message = messageCancel
            };
        }
    }
}