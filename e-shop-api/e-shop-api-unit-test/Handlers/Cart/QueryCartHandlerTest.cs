using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Applications.Cart.CommonDto;
using e_shop_api.Applications.Cart.Query;
using e_shop_api.Extensions;
using e_shop_api.Utility.Dto;
using e_shop_api.Utility.Interface;
using e_shop_api_unit_test.Utility;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace e_shop_api_unit_test.Handlers.Cart;

public class QueryCartHandlerTest : TestBase
{
    private QueryCartHandler _target;
    private readonly ILogger<QueryCartHandler> _fakeLog;
    private readonly IShoppingCartUtility _fakeShoppingCartUtility;

    public QueryCartHandlerTest()
    {
        #region 測試前置資料

        // product
        FakeEShopDbContext.Products.Add(new e_shop_api.DataBase.Models.Product()
        {
            Id = 1,
            Category = "金牌",
            Content = "TestContent",
            Description = "TestDescription",
            ImageUrl = "TestUrl",
            IsEnabled = true,
            Num = 50,
            OriginPrice = 299,
            Price = 250,
            Title = "TestProduct",
            Unit = "件"
        });
        // coupon
        // 85折
        FakeEShopDbContext.Coupons.Add(new e_shop_api.DataBase.Models.Coupon()
        {
            Id = 1,
            Title = "測試優惠",
            CouponCode = "Test123",
            Percent = 85,
            IsEnabled = true,
            DueDateTime = new DateTime(2222, 12, 31)
        });
        // 95折
        FakeEShopDbContext.Coupons.Add(new e_shop_api.DataBase.Models.Coupon()
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
        // shopping cart
        _fakeShoppingCartUtility = Substitute.For<IShoppingCartUtility>();

        _target = new QueryCartHandler(FakeEShopDbContext, _fakeShoppingCartUtility, _fakeLog);
    }

    [Fact]
    public async Task 當購物車有資料且有使用85折優惠券時_返回的FinalTotalAmount為TotalAmount的85折()
    {
        // 購物車的資料
        _fakeShoppingCartUtility.GetShoppingItemsFromCart(Arg.Any<string>()).Returns(
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
        _fakeShoppingCartUtility.GetCouponIdFromCart(Arg.Any<string>()).Returns(1);

        _target = new QueryCartHandler(FakeEShopDbContext, _fakeShoppingCartUtility, _fakeLog);

        var request = new QueryCartRequest();
        var expected = new QueryCartResponse()
        {
            Success = true,
            Message = "查詢成功!",
            Carts = new List<e_shop_api.Applications.Cart.CommonDto.Cart>()
            {
                new e_shop_api.Applications.Cart.CommonDto.Cart()
                {
                    CartDetailId = "test123",
                    Product = new ShoppingProduct()
                    {
                        ProductId = 1,
                        Category = "金牌",
                        Content = "TestContent",
                        Description = "TestDescription",
                        ImageUrl = "TestUrl",
                        IsEnabled = true,
                        Num = 50,
                        OriginPrice = 299,
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
        _fakeShoppingCartUtility.GetShoppingItemsFromCart(Arg.Any<string>()).Returns(
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
        _fakeShoppingCartUtility.GetCouponIdFromCart(Arg.Any<string>()).Returns(2);

        _target = new QueryCartHandler(FakeEShopDbContext, _fakeShoppingCartUtility, _fakeLog);

        var request = new QueryCartRequest();
        var expected = new QueryCartResponse()
        {
            Success = true,
            Message = "查詢成功!",
            Carts = new List<e_shop_api.Applications.Cart.CommonDto.Cart>()
            {
                new e_shop_api.Applications.Cart.CommonDto.Cart()
                {
                    CartDetailId = "test123",
                    Product = new ShoppingProduct()
                    {
                        ProductId = 1,
                        Category = "金牌",
                        Content = "TestContent",
                        Description = "TestDescription",
                        ImageUrl = "TestUrl",
                        IsEnabled = true,
                        Num = 50,
                        OriginPrice = 299,
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
        _fakeShoppingCartUtility.GetShoppingItemsFromCart(Arg.Any<string>()).Returns(
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
        _fakeShoppingCartUtility.GetCouponIdFromCart(Arg.Any<string>()).Returns((int?)null);

        _target = new QueryCartHandler(FakeEShopDbContext, _fakeShoppingCartUtility, _fakeLog);

        var request = new QueryCartRequest();
        var expected = new QueryCartResponse()
        {
            Success = true,
            Message = "查詢成功!",
            Carts = new List<e_shop_api.Applications.Cart.CommonDto.Cart>()
            {
                new e_shop_api.Applications.Cart.CommonDto.Cart()
                {
                    CartDetailId = "test123",
                    Product = new ShoppingProduct()
                    {
                        ProductId = 1,
                        Category = "金牌",
                        Content = "TestContent",
                        Description = "TestDescription",
                        ImageUrl = "TestUrl",
                        IsEnabled = true,
                        Num = 50,
                        OriginPrice = 299,
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
        _fakeShoppingCartUtility.GetShoppingItemsFromCart(Arg.Any<string>()).Returns(
            new List<ShoppingCartItemCache>());
        // 設定沒有使用優惠券
        _fakeShoppingCartUtility.GetCouponIdFromCart(Arg.Any<string>()).Returns((int?)null);

        _target = new QueryCartHandler(FakeEShopDbContext, _fakeShoppingCartUtility, _fakeLog);

        var request = new QueryCartRequest();
        var expected = new QueryCartResponse()
        {
            Success = false,
            Message = "購物車沒有資料!",
            Carts = new List<e_shop_api.Applications.Cart.CommonDto.Cart>(),
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
        _fakeShoppingCartUtility.GetShoppingItemsFromCart(Arg.Any<string>()).Returns(
            new List<ShoppingCartItemCache>());
        // 設定使用95折優惠券
        _fakeShoppingCartUtility.GetCouponIdFromCart(Arg.Any<string>()).Returns(2);

        _target = new QueryCartHandler(FakeEShopDbContext, _fakeShoppingCartUtility, _fakeLog);

        var request = new QueryCartRequest();
        var expected = new QueryCartResponse()
        {
            Success = false,
            Message = "購物車沒有資料!",
            Carts = new List<e_shop_api.Applications.Cart.CommonDto.Cart>(),
            TotalAmount = 0,
            FinalTotalAmount = 0
        };

        var actual = await _target.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }
}