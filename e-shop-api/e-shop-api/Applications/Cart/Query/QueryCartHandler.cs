using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Applications.Cart.CommonDto;
using e_shop_api.Core.Extensions;
using e_shop_api.DataBase;
using e_shop_api.Extensions;
using e_shop_api.Utility.Const;
using e_shop_api.Utility.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Cart.Query
{
    public class QueryCartHandler : IRequestHandler<QueryCartRequest, QueryCartResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly IShoppingCartUtility _shoppingCartUtility;
        private readonly ILogger<QueryCartHandler> _logger;

        public QueryCartHandler(EShopDbContext eShopDbContext, IShoppingCartUtility shoppingCartUtility,
            ILogger<QueryCartHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _shoppingCartUtility = shoppingCartUtility;
            _logger = logger;
        }

        public async Task<QueryCartResponse> Handle(QueryCartRequest request, CancellationToken cancellationToken)
        {
            // product
            var productList = await _eShopDbContext.Products
                .Select(s => new ShoppingProduct()
                {
                    Category = s.Category,
                    Content = s.Content,
                    Description = s.Description,
                    ProductId = s.Id,
                    ImageUrl = s.ImageUrl,
                    IsEnabled = s.IsEnabled,
                    OriginPrice = s.OriginPrice,
                    Price = s.Price,
                    Title = s.Title,
                    Unit = s.Unit,
                    Num = s.Num
                }).ToListAsync(cancellationToken: cancellationToken);

            // coupon
            var couponList = await _eShopDbContext.Coupons
                .Select(s => new ShoppingCoupon()
                {
                    CouponId = s.Id,
                    Title = s.Title,
                    CouponCode = s.CouponCode,
                    Percent = s.Percent,
                    IsEnabled = s.IsEnabled,
                    DueDateTimeStamp = s.DueDateTime.ToTimeStamp()
                }).ToListAsync(cancellationToken: cancellationToken);

            // shopping items
            var shoppingItems = _shoppingCartUtility.GetShoppingItemsFromCart(CartInfo.DefaultCartId);

            if (shoppingItems.Any() == false)
            {
                return new QueryCartResponse()
                {
                    Success = false,
                    Message = "購物車沒有資料!",
                    Carts = new List<CommonDto.Cart>(),
                    TotalAmount = 0,
                    FinalTotalAmount = 0
                };
            }

            // use coupon
            var couponId = _shoppingCartUtility.GetCouponIdFromCart(CartInfo.DefaultCartId);
            var coupon = couponList.SingleOrDefault(s => s.CouponId == couponId);
            // temp
            var shoppingCarts = new List<CommonDto.Cart>();
            var shoppingTotalAmount = 0m;
            var shoppingFinalTotalAmount = 0m;
            foreach (var shoppingItem in shoppingItems)
            {
                var product = productList.Single(s => s.ProductId == shoppingItem.ProductId);
                var cartDetail = new CommonDto.Cart()
                {
                    CartDetailId = shoppingItem.ShoppingItemId,
                    Product = product,
                    // 如果為null，則表示沒有使用coupon
                    Coupon = coupon,
                    Qty = shoppingItem.Qty
                };

                shoppingCarts.Add(cartDetail);
                shoppingTotalAmount += (product.Price * shoppingItem.Qty);
            }

            if (coupon != null)
            {
                var couponPercent = coupon.Percent / 100f;
                shoppingFinalTotalAmount = shoppingTotalAmount * (decimal)couponPercent;
            }
            else
            {
                shoppingFinalTotalAmount = shoppingTotalAmount;
            }

            return new QueryCartResponse()
            {
                Success = true,
                Message = "查詢成功!",
                Carts = shoppingCarts,
                TotalAmount = shoppingTotalAmount,
                FinalTotalAmount = shoppingFinalTotalAmount
            };
        }
    }
}