using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Core.Dto.Cart;
using e_shop_api.Core.Extensions;
using e_shop_api_unit_test.Utility;
using EShop.Cache.Dto;
using EShop.Cache.Interface;
using EShop.Logic.Applications.Cart.CommonDto;
using EShop.Logic.Applications.Cart.Query;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace e_shop_api_unit_test.Handlers.Cart;

public class QueryCartHandlerTest : TestBase
{
    private QueryCartHandler _target;
    private readonly ILogger<QueryCartHandler> _fakeLog;
    private readonly IShoppingCartCacheUtility _fakeShoppingCartCacheUtility;
    private readonly IProductsCacheUtility _fakeProductsCacheUtility;

    public QueryCartHandlerTest()
    {
        #region 測試前置資料

        // coupon
        // 85折
        FakeEShopDbContext.Coupons.Add(new EShop.Entity.DataBase.Models.Coupon()
        {
            Id = 1,
            Title = "測試優惠",
            CouponCode = "Test123",
            Percent = 85,
            IsEnabled = true,
            DueDateTime = new DateTime(2222, 12, 31)
        });
        // 95折
        FakeEShopDbContext.Coupons.Add(new EShop.Entity.DataBase.Models.Coupon()
        {
            Id = 2,
            Title = "測試優惠2",
            CouponCode = "Test123",
            Percent = 95,
            IsEnabled = true,
            DueDateTime = new DateTime(2222, 12, 31)
        });
        FakeEShopDbContext.SaveChanges();

        #endregion

        // log
        _fakeLog = Substitute.For<ILogger<QueryCartHandler>>();
        // shopping cart cache
        _fakeShoppingCartCacheUtility = Substitute.For<IShoppingCartCacheUtility>();
        // products cache
        _fakeProductsCacheUtility = Substitute.For<IProductsCacheUtility>();
        _fakeProductsCacheUtility.GetProductInfo(1).Returns(new ShoppingProduct()
        {
            ProductId = 1,
            ImageUrl = "TestUrl",
            Price = 250,
            Title = "TestProduct",
            Unit = "件"
        });

        _target = new QueryCartHandler(FakeEShopDbContext, _fakeShoppingCartCacheUtility, _fakeProductsCacheUtility,
            _fakeLog);
    }

    [Fact]
    public async Task 當購物車有資料且有使用85折優惠券時_返回的FinalTotalAmount為TotalAmount的85折()
    {
        // 購物車的資料
        _fakeShoppingCartCacheUtility.GetShoppingItemsFromCart(Arg.Any<string>()).Returns(
            new List<ShoppingCartItemCache>()
            {
                new ShoppingCartItemCache()
                {
                    ShoppingItemId = "test123",
                    ProductId = 1,
                    Qty = 10
                }
            });
        // 設定使用85折優惠券
        _fakeShoppingCartCacheUtility.GetCouponIdFromCart(Arg.Any<string>()).Returns(1);

        _target = new QueryCartHandler(FakeEShopDbContext, _fakeShoppingCartCacheUtility, _fakeProductsCacheUtility,
            _fakeLog);

        var request = new QueryCartRequest();
        var expected = new QueryCartResponse()
        {
            Success = true,
            Message = "查詢成功!",
            Carts = new List<EShop.Logic.Applications.Cart.CommonDto.Cart>()
            {
                new EShop.Logic.Applications.Cart.CommonDto.Cart()
                {
                    CartDetailId = "test123",
                    Product = new ShoppingProduct()
                    {
                        ProductId = 1,
                        ImageUrl = "TestUrl",
                        Price = 250,
                        Title = "TestProduct",
                        Unit = "件"
                    },
                    Coupon = new ShoppingCoupon()
                    {
                        CouponId = 1,
                        Title = "測試優惠",
                        CouponCode = "Test123",
                        Percent = 85,
                        IsEnabled = true,
                        DueDateTimeStamp = new DateTime(2222, 12, 31).ToTimeStamp()
                    },
                    Qty = 10
                }
            },
            TotalAmount = 2500,
            FinalTotalAmount = 2125
        };

        var actual = await _target.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task 當購物車有資料且有使用95折優惠券時_返回的FinalTotalAmount為TotalAmount的95折()
    {
        // 購物車的資料
        _fakeShoppingCartCacheUtility.GetShoppingItemsFromCart(Arg.Any<string>()).Returns(
            new List<ShoppingCartItemCache>()
            {
                new ShoppingCartItemCache()
                {
                    ShoppingItemId = "test123",
                    ProductId = 1,
                    Qty = 20
                }
            });
        // 設定使用95折優惠券
        _fakeShoppingCartCacheUtility.GetCouponIdFromCart(Arg.Any<string>()).Returns(2);

        _target = new QueryCartHandler(FakeEShopDbContext, _fakeShoppingCartCacheUtility, _fakeProductsCacheUtility,
            _fakeLog);

        var request = new QueryCartRequest();
        var expected = new QueryCartResponse()
        {
            Success = true,
            Message = "查詢成功!",
            Carts = new List<EShop.Logic.Applications.Cart.CommonDto.Cart>()
            {
                new EShop.Logic.Applications.Cart.CommonDto.Cart()
                {
                    CartDetailId = "test123",
                    Product = new ShoppingProduct()
                    {
                        ProductId = 1,
                        ImageUrl = "TestUrl",
                        Price = 250,
                        Title = "TestProduct",
                        Unit = "件"
                    },
                    Coupon = new ShoppingCoupon()
                    {
                        CouponId = 2,
                        Title = "測試優惠2",
                        CouponCode = "Test123",
                        Percent = 95,
                        IsEnabled = true,
                        DueDateTimeStamp = new DateTime(2222, 12, 31).ToTimeStamp()
                    },
                    Qty = 20
                }
            },
            TotalAmount = 5000,
            FinalTotalAmount = 4750
        };

        var actual = await _target.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task 當購物車有資料且沒有使用優惠券時_返回的FinalTotalAmount和TotalAmount相等()
    {
        // 購物車的資料
        _fakeShoppingCartCacheUtility.GetShoppingItemsFromCart(Arg.Any<string>()).Returns(
            new List<ShoppingCartItemCache>()
            {
                new ShoppingCartItemCache()
                {
                    ShoppingItemId = "test123",
                    ProductId = 1,
                    Qty = 20
                }
            });
        // 設定沒有使用優惠券
        _fakeShoppingCartCacheUtility.GetCouponIdFromCart(Arg.Any<string>()).Returns((int?)null);

        _target = new QueryCartHandler(FakeEShopDbContext, _fakeShoppingCartCacheUtility, _fakeProductsCacheUtility,
            _fakeLog);

        var request = new QueryCartRequest();
        var expected = new QueryCartResponse()
        {
            Success = true,
            Message = "查詢成功!",
            Carts = new List<EShop.Logic.Applications.Cart.CommonDto.Cart>()
            {
                new EShop.Logic.Applications.Cart.CommonDto.Cart()
                {
                    CartDetailId = "test123",
                    Product = new ShoppingProduct()
                    {
                        ProductId = 1,
                        ImageUrl = "TestUrl",
                        Price = 250,
                        Title = "TestProduct",
                        Unit = "件"
                    },
                    Coupon = null,
                    Qty = 20
                }
            },
            TotalAmount = 5000,
            FinalTotalAmount = 5000
        };

        var actual = await _target.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task 當購物車沒有資料且沒有使用優惠券時_返回購物車沒有資料()
    {
        // 購物車沒有資料
        _fakeShoppingCartCacheUtility.GetShoppingItemsFromCart(Arg.Any<string>()).Returns(
            new List<ShoppingCartItemCache>());
        // 設定沒有使用優惠券
        _fakeShoppingCartCacheUtility.GetCouponIdFromCart(Arg.Any<string>()).Returns((int?)null);

        _target = new QueryCartHandler(FakeEShopDbContext, _fakeShoppingCartCacheUtility, _fakeProductsCacheUtility,
            _fakeLog);

        var request = new QueryCartRequest();
        var expected = new QueryCartResponse()
        {
            Success = false,
            Message = "購物車沒有資料!",
            Carts = new List<EShop.Logic.Applications.Cart.CommonDto.Cart>(),
            TotalAmount = 0,
            FinalTotalAmount = 0
        };

        var actual = await _target.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task 當購物車沒有資料且有使用95折優惠券時_返回購物車沒有資料()
    {
        // 購物車沒有資料
        _fakeShoppingCartCacheUtility.GetShoppingItemsFromCart(Arg.Any<string>()).Returns(
            new List<ShoppingCartItemCache>());
        // 設定使用95折優惠券
        _fakeShoppingCartCacheUtility.GetCouponIdFromCart(Arg.Any<string>()).Returns(2);

        _target = new QueryCartHandler(FakeEShopDbContext, _fakeShoppingCartCacheUtility, _fakeProductsCacheUtility,
            _fakeLog);

        var request = new QueryCartRequest();
        var expected = new QueryCartResponse()
        {
            Success = false,
            Message = "購物車沒有資料!",
            Carts = new List<EShop.Logic.Applications.Cart.CommonDto.Cart>(),
            TotalAmount = 0,
            FinalTotalAmount = 0
        };

        var actual = await _target.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }
}