using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Core.Dto;
using e_shop_api.Core.Extensions;
using e_shop_api.Core.Utility;
using e_shop_api_unit_test.Utility;
using EShop.Logic.Applications.Coupon.Query;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace e_shop_api_unit_test.Handlers.Coupon;

public class QueryCouponsHandlerTest : TestBase
{
    private QueryCouponsHandler _target;

    [Fact]
    public async Task 查詢優惠券清單_返回資料()
    {
        // Arrange
        var fakeLog = Substitute.For<ILogger<QueryCouponsHandler>>();
        FakeEShopDbContext.Coupons.Add(new EShop.Entity.DataBase.Models.Coupon()
        {
            Id = 1,
            CouponCode = "test123",
            DueDateTime = new DateTime(2021, 10, 30),
            IsEnabled = true,
            Percent = 95,
            Title = "測試優惠"
        });
        await FakeEShopDbContext.SaveChangesAsync();
        _target = new QueryCouponsHandler(FakeEShopDbContext, fakeLog, new PageUtility());

        var request = new QueryCouponsRequest()
        {
            Page = 1,
            PageSize = 10
        };
        var expected = new QueryCouponsResponse()
        {
            Success = true,
            Message = "查詢成功",
            Coupons = new List<EShop.Logic.Applications.Coupon.CommonDto.Coupon>()
            {
                new EShop.Logic.Applications.Coupon.CommonDto.Coupon()
                {
                    CouponId = 1,
                    CouponCode = "test123",
                    DueDateTimeStamp = new DateTime(2021, 10, 30).ToTimeStamp(),
                    IsEnabled = true,
                    Percent = 95,
                    Title = "測試優惠"
                }
            },
            Pagination = new Pagination()
            {
                CurrentPage = 1,
                HasNextPage = false,
                HasPrePage = false,
                TotalPages = 1,
                PageSize = 10
            }
        };

        // Act
        var actual = await _target.Handle(request, CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task 查詢優惠券清單_查無資料()
    {
        // Arrange
        var fakeLog = Substitute.For<ILogger<QueryCouponsHandler>>();
        _target = new QueryCouponsHandler(FakeEShopDbContext, fakeLog, new PageUtility());

        var request = new QueryCouponsRequest()
        {
            Page = 1,
            PageSize = 10
        };
        var expected = new QueryCouponsResponse()
        {
            Success = false,
            Message = "沒有資料",
            Coupons = new List<EShop.Logic.Applications.Coupon.CommonDto.Coupon>(),
            Pagination = new Pagination()
        };

        // Act
        var actual = await _target.Handle(request, CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}