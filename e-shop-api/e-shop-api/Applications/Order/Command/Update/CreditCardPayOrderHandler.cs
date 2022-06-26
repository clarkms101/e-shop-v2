using System;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.DataBase;
using e_shop_api.Enumeration;
using MediatR;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Order.Command.Update
{
    public class CreditCardPayOrderHandler : IRequestHandler<CreditCardPayOrderRequest, CreditCardPayOrderResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<CreditCardPayOrderHandler> _logger;

        public CreditCardPayOrderHandler(EShopDbContext eShopDbContext, ILogger<CreditCardPayOrderHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
        }

        public async Task<CreditCardPayOrderResponse> Handle(CreditCardPayOrderRequest request,
            CancellationToken cancellationToken)
        {
            // 更新訂單狀態 (模擬已經收到第三方金流處理完成的訊息，再更新訂單的支付狀態)
            var order = await _eShopDbContext.Orders.FindAsync(request.OrderId);
            order.IsPaid = true;
            order.PaymentMethod = PaymentMethod.CreditCardPayment.ToString();
            order.PaidDateTime = DateTime.Now;

            _eShopDbContext.Orders.Update(order);
            await _eShopDbContext.SaveChangesAsync(cancellationToken);

            return new CreditCardPayOrderResponse()
            {
                Success = true,
                Message = "信用卡支付成功!"
            };
        }
    }
}