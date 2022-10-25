using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Core.Enumeration;
using e_shop_api.Core.Extensions;
using e_shop_api_unit_test.Utility;
using EShop.Entity.DataBase.Models;
using EShop.Logic.Applications.Order.CommonDto;
using EShop.Logic.Applications.Order.Query;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace e_shop_api_unit_test.Handlers.Order;

public class QueryOrderHandlerTest : TestBase
{
    private readonly QueryOrderHandler _target;

    public QueryOrderHandlerTest()
    {
        var fakeLog = Substitute.For<ILogger<QueryOrderHandler>>();

        // orders
        FakeEShopDbContext.Orders.Add(new EShop.Entity.DataBase.Models.Order()
        {
            Id = 1,
            SerialNumber = "fb39c58d-c94c-4362-8a83-cbee0a1f50d7",
            UserId = null,
            UserName = "Clark",
            Address = "TestAddress",
            CreationTime = new DateTime(2021, 10, 30),
            Email = "TestEmail",
            IsPaid = true,
            Message = "TestMessage",
            PaidDateTime = new DateTime(2021, 10, 31),
            PaymentMethod = PaymentMethod.CashOnDelivery.ToString(),
            OrderStatus = OrderStatus.Created.ToString(),
            Tel = "123456789",
            TotalAmount = 1999
        });
        FakeEShopDbContext.Orders.Add(new EShop.Entity.DataBase.Models.Order()
        {
            Id = 2,
            SerialNumber = "a5ccb6a1-2eb3-4683-bc7b-a01d2a6b4351",
            UserId = null,
            UserName = "Clark",
            Address = "TestAddress2",
            CreationTime = new DateTime(2021, 11, 25),
            Email = "TestEmail2",
            IsPaid = true,
            Message = "TestMessage2",
            PaidDateTime = new DateTime(2021, 11, 26),
            PaymentMethod = PaymentMethod.CashOnDelivery.ToString(),
            OrderStatus = OrderStatus.Created.ToString(),
            Tel = "123456789",
            TotalAmount = 2999
        });
        // order detail
        FakeEShopDbContext.OrderDetails.Add(new OrderDetail()
        {
            Id = 1,
            OrderId = 1,
            ProductId = 1,
            Qty = 5
        });
        // product
        FakeEShopDbContext.Products.Add(new EShop.Entity.DataBase.Models.Product()
        {
            Id = 1,
            Category = "金牌",
            Content = "TestContent",
            Description = "TestDescription",
            ImageUrl = "TestUrl",
            IsEnabled = true,
            Num = 50,
            OriginPrice = 990,
            Price = 880,
            Title = "TestProduct",
            Unit = "件"
        });

        FakeEShopDbContext.SaveChanges();

        _target = new QueryOrderHandler(FakeEShopDbContext, fakeLog);
    }

    [Fact]
    public async Task 查詢已存在的訂單編號_返回該筆訂單的資料()
    {
        var request = new QueryOrderRequest()
        {
            SerialNumber = "fb39c58d-c94c-4362-8a83-cbee0a1f50d7"
        };
        var expected = new QueryOrderResponse()
        {
            Success = true,
            Message = "查詢成功!",
            OrderInfo = new OrderInfo()
            {
                OrderId = 1,
                UserId = null,
                IsPaid = true,
                PaymentMethod = PaymentMethod.CashOnDelivery.GetDescriptionText(),
                OrderStatus = OrderStatus.Created.GetDescriptionText(),
                CreateDateTime = new DateTime(2021, 10, 30).ToTimeStamp(),
                PaidDateTime = new DateTime(2021, 10, 31).ToTimeStamp(),
                OriginTotalAmount = 4400,
                TotalAmount = 1999,
                UserName = "Clark",
                Address = "TestAddress",
                Email = "TestEmail",
                Tel = "123456789",
                Message = "TestMessage",
                OrderDetailInfos = new List<OrderDetailInfo>()
                {
                    new OrderDetailInfo()
                    {
                        ProductTitle = "TestProduct",
                        ProductUnit = "件",
                        ImageUrl = "TestUrl",
                        ProductPrice = 880,
                        Qty = 5
                    }
                }
            }
        };

        var actual = await _target.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task 查詢不存在的訂單編號_返回查無該筆資料()
    {
        var request = new QueryOrderRequest()
        {
            SerialNumber = "ed4aa83c-9fb9-4577-a4af-4aaa8a579131"
        };
        var expected = new QueryOrderResponse()
        {
            Success = false,
            Message = "查無該筆資料!"
        };

        var actual = await _target.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }
}