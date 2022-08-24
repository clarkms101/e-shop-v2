using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Applications;
using e_shop_api.Applications.Product.Query;
using e_shop_api.Utility;
using e_shop_api_unit_test.Utility;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace e_shop_api_unit_test.Handlers.Product;

public class QueryProductsHandlerTest : TestBase
{
    private readonly QueryProductsHandler _target;

    public QueryProductsHandlerTest()
    {
        var fakeLog = Substitute.For<ILogger<QueryProductsHandler>>();
        FakeEShopDbContext.Products.Add(new e_shop_api.DataBase.Models.Product()
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
        FakeEShopDbContext.Products.Add(new e_shop_api.DataBase.Models.Product()
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
        _target = new QueryProductsHandler(FakeEShopDbContext, fakeLog, new PageUtility());
    }

    [Fact]
    public async Task 查詢產品清單_返回所有的產品資料()
    {
        // Arrange
        var request = new QueryProductsRequest()
        {
            Category = "",
            Page = 1,
            PageSize = 10
        };
        var expected = new QueryProductsResponse()
        {
            Success = true,
            Message = "查詢成功",
            Products = new List<e_shop_api.Applications.Product.CommonDto.Product>()
            {
                new e_shop_api.Applications.Product.CommonDto.Product()
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
                },
                new e_shop_api.Applications.Product.CommonDto.Product()
                {
                    ProductId = 2,
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
                },
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
    public async Task 查詢產品清單且為寵物用品分類_返回指定分類的產品資料()
    {
        // Arrange
        var request = new QueryProductsRequest()
        {
            Category = "寵物用品",
            Page = 1,
            PageSize = 10
        };
        var expected = new QueryProductsResponse()
        {
            Success = true,
            Message = "查詢成功",
            Products = new List<e_shop_api.Applications.Product.CommonDto.Product>()
            {
                new e_shop_api.Applications.Product.CommonDto.Product()
                {
                    ProductId = 2,
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
    public async Task 查詢產品清單且為default分類_返回指定分類的產品資料()
    {
        // Arrange
        const string defaultCategory = "金牌";
        var request = new QueryProductsRequest()
        {
            Category = "default",
            Page = 1,
            PageSize = 10
        };
        var expected = new QueryProductsResponse()
        {
            Success = true,
            Message = "查詢成功",
            Products = new List<e_shop_api.Applications.Product.CommonDto.Product>()
            {
                new e_shop_api.Applications.Product.CommonDto.Product()
                {
                    ProductId = 1,
                    Category = defaultCategory,
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
}