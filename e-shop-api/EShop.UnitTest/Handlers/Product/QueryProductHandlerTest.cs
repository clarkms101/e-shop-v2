using System.Threading;
using System.Threading.Tasks;
using e_shop_api_unit_test.Utility;
using EShop.Logic.Applications.Product.Query;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace e_shop_api_unit_test.Handlers.Product;

public class QueryProductHandlerTest : TestBase
{
    private readonly QueryProductHandler _target;

    /// <summary>
    /// 測試的前置作業
    /// </summary>
    public QueryProductHandlerTest()
    {
        var fakeLog = Substitute.For<ILogger<QueryProductHandler>>();

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
            Unit = "個"
        });
        FakeEShopDbContext.Products.Add(new EShop.Entity.DataBase.Models.Product()
        {
            Id = 2,
            Category = "寵物用品",
            Content = "TestContent",
            Description = "TestDescription",
            ImageUrl = "TestUrl",
            IsEnabled = true,
            Num = 70,
            OriginPrice = 1990,
            Price = 1880,
            Title = "TestProduct2",
            Unit = "個"
        });
        FakeEShopDbContext.SaveChanges();

        _target = new QueryProductHandler(FakeEShopDbContext, fakeLog);
    }

    [Fact]
    public async Task 查詢指定的產品編號_返回該產品的資訊()
    {
        // Arrange
        var request = new QueryProductRequest()
        {
            ProductId = 1
        };
        var expected = new QueryProductResponse()
        {
            Success = true,
            Message = "查詢成功!",
            Product = new EShop.Logic.Applications.Product.CommonDto.Product()
            {
                ProductId = 1,
                Category = "金牌",
                Content = "TestContent",
                Description = "TestDescription",
                ImageUrl = "TestUrl",
                IsEnabled = true,
                Num = 50,
                OriginPrice = 990,
                Price = 880,
                Title = "TestProduct",
                Unit = "個"
            }
        };

        // Act
        var actual = await _target.Handle(request, CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task 查詢不存在的產品編號_返回錯誤訊息()
    {
        // Arrange
        var request = new QueryProductRequest()
        {
            ProductId = 99
        };
        var expected = new QueryProductResponse()
        {
            Success = false,
            Message = "查無該筆資料!"
        };

        // Act
        var actual = await _target.Handle(request, CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}