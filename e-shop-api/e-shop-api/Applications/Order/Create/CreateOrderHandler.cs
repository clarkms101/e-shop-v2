using System;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Applications.Cart.Command.Update;
using e_shop_api.Applications.Cart.Query;
using e_shop_api.DataBase;
using e_shop_api.DataBase.Models;
using e_shop_api.Utility.Const;
using MediatR;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Order.Create
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<CreateOrderHandler> _logger;
        private readonly QueryCartHandler _queryCartHandler;
        private readonly CleanCartHandler _cleanCartHandler;

        public CreateOrderHandler(
            EShopDbContext eShopDbContext,
            ILogger<CreateOrderHandler> logger,
            QueryCartHandler queryCartHandler,
            CleanCartHandler cleanCartHandler)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
            _queryCartHandler = queryCartHandler;
            _cleanCartHandler = cleanCartHandler;
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            // create order
            var shoppingCart = await _queryCartHandler.Handle(new QueryCartRequest(), cancellationToken);
            var newOrder = new DataBase.Models.Order()
            {
                UserId = null,
                IsPaid = false,
                PaymentMethod = string.IsNullOrWhiteSpace(request.OrderForm.PaymentMethod)
                    ? null
                    : request.OrderForm.PaymentMethod,
                CreationTime = DateTime.Now,
                PaidDateTime = null,
                TotalAmount = shoppingCart.FinalTotalAmount,
                UserName = request.OrderForm.UserName,
                Address = request.OrderForm.Address,
                Email = request.OrderForm.Email,
                Tel = request.OrderForm.Tel,
                Message = request.OrderForm.Message
            };

            await _eShopDbContext.Orders.AddAsync(newOrder, cancellationToken);
            await _eShopDbContext.SaveChangesAsync(cancellationToken);

            // create order detail
            foreach (var cartDetail in shoppingCart.Carts)
            {
                var orderDetail = new OrderDetail()
                {
                    OrderId = newOrder.Id,
                    ProductId = cartDetail.Product.ProductId,
                    Qty = cartDetail.Qty
                };
                await _eShopDbContext.OrderDetails.AddAsync(orderDetail, cancellationToken);
            }

            await _eShopDbContext.SaveChangesAsync(cancellationToken);

            // clean shopping cart
            await _cleanCartHandler.Handle(new CleanCartRequest()
            {
                ShoppingCartId = CartInfo.DefaultCartId
            }, cancellationToken);

            return new CreateOrderResponse()
            {
                Success = true,
                Message = "訂單已建立!",
                OrderId = newOrder.Id
            };
        }
    }
}