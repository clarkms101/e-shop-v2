using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Applications.Cart.Command.Update;
using e_shop_api.Applications.Cart.Query;
using e_shop_api.Core.Const;
using e_shop_api.Core.Enumeration;
using EShop.Entity.DataBase;
using EShop.Entity.DataBase.Models;
using EShop.MQ.Producer;
using MediatR;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Order.Command.Create
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<CreateOrderHandler> _logger;
        private readonly QueryCartHandler _queryCartHandler;
        private readonly CleanCartHandler _cleanCartHandler;
        private readonly MqProducer _mqProducer;

        public CreateOrderHandler(
            EShopDbContext eShopDbContext,
            ILogger<CreateOrderHandler> logger,
            QueryCartHandler queryCartHandler,
            CleanCartHandler cleanCartHandler,
            MqProducer mqProducer)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
            _queryCartHandler = queryCartHandler;
            _cleanCartHandler = cleanCartHandler;
            _mqProducer = mqProducer;
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            // create order
            var shoppingCart = await _queryCartHandler.Handle(new QueryCartRequest(), cancellationToken);
            var newOrder = new EShop.Entity.DataBase.Models.Order()
            {
                SerialNumber = Guid.NewGuid().ToString(),
                UserId = null,
                IsPaid = false,
                OrderStatus = OrderStatus.Created.ToString(),
                PaymentMethod = string.IsNullOrWhiteSpace(request.OrderForm.PaymentMethod)
                    ? null
                    : request.OrderForm.PaymentMethod,
                PaidDateTime = null,
                TotalAmount = shoppingCart.FinalTotalAmount,
                UserName = request.OrderForm.UserName,
                Address = request.OrderForm.Address,
                Email = request.OrderForm.Email,
                Tel = request.OrderForm.Tel,
                Message = request.OrderForm.Message,
                // system
                CreatorUserId = 0, // 系統編號
                CreationTime = DateTime.Now
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
                    Qty = cartDetail.Qty,
                    // system
                    CreatorUserId = 0, // 系統編號
                    CreationTime = DateTime.Now
                };
                await _eShopDbContext.OrderDetails.AddAsync(orderDetail, cancellationToken);
            }

            // update product 庫存 todo
            // foreach (var item in shoppingCart.Carts)
            // {
            //     var product = _eShopDbContext.Products.Single(s => s.Id == item.Product.ProductId);
            //     product.Num -= item.Qty;
            //     product.LastModificationTime = DateTime.Now;
            // }

            await _eShopDbContext.SaveChangesAsync(cancellationToken);

            // clean shopping cart
            await _cleanCartHandler.Handle(new CleanCartRequest()
            {
                ShoppingCartId = CartInfo.DefaultCartId
            }, cancellationToken);

            // 加入訂單自動取消確認排程
            await _mqProducer.SetOrderAutoCancelSchedule(newOrder.Id);

            return new CreateOrderResponse()
            {
                Success = true,
                Message = "訂單已建立!",
                SerialNumber = newOrder.SerialNumber
            };
        }
    }
}