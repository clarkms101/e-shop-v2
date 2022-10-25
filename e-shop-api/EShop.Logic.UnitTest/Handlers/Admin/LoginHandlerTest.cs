using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Core.Enumeration;
using e_shop_api.Core.Utility.Dto;
using e_shop_api.Core.Utility.Interface;
using e_shop_api_unit_test.Utility;
using EShop.Cache.Interface;
using EShop.Logic.Applications.Admin.Command.Login;
using EShop.Logic.Utility;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace e_shop_api_unit_test.Handlers.Admin;

public class LoginHandlerTest : TestBase
{
    private readonly LoginHandler _target;
    private const long FakeExpireTimeStamp = 12345678;

    /// <summary>
    /// 測試的前置作業
    /// </summary>
    public LoginHandlerTest()
    {
        // log
        var fakeLog = Substitute.For<ILogger<LoginHandler>>();

        // jwt
        var fakeJwtUtility = Substitute.For<IJwtUtility>();
        fakeJwtUtility.GenerateAdminToken(Arg.Any<AdminInfo>())
            .Returns("testJwtToken");
        fakeJwtUtility.GetExpiredTimeStamp()
            .Returns(FakeExpireTimeStamp);

        // cache
        var fakeMemoryCacheUtility = Substitute.For<IMemoryCacheUtility>();

        // http header
        var fakeHttpContextAccessor = Substitute.For<IHttpContextAccessor>();
        var context = new DefaultHttpContext();
        context.Request.Headers["User-Agent"] = "testWeb";
        fakeHttpContextAccessor.HttpContext.Returns(context);

        // db
        FakeEShopDbContext.Admins.Add(new EShop.Entity.DataBase.Models.Admin()
        {
            Id = 2,
            Account = "Tom",
            Password = "cc03e747a6afbbcbf8be7668acfebee5",
            Permission = Permission.Public
        });
        FakeEShopDbContext.SaveChanges();

        _target = new LoginHandler(FakeEShopDbContext, fakeJwtUtility, fakeMemoryCacheUtility, fakeLog,
            fakeHttpContextAccessor);
    }

    [Fact]
    public async Task 輸入正確的帳號和密碼_返回登入成功()
    {
        // Arrange
        var request = new LoginRequest()
        {
            Account = "Tom",
            Password = "test123"
        };
        var expected = new LoginResponse()
        {
            Success = true,
            Message = "登入成功!",
            Token = "testJwtToken",
            ExpiredTimeStamp = FakeExpireTimeStamp
        };

        // Act
        var actual = await _target.Handle(request, CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task 輸入錯誤的帳號_返回登入失敗()
    {
        // Arrange
        var request = new LoginRequest()
        {
            Account = "Eric",
            Password = "test123"
        };
        var expected = new LoginResponse()
        {
            Success = false,
            Message = "帳密錯誤!",
            Token = null,
            ExpiredTimeStamp = 0L
        };

        // Act
        var actual = await _target.Handle(request, CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task 輸入錯誤的密碼_返回登入失敗()
    {
        // Arrange
        var request = new LoginRequest()
        {
            Account = "Tom",
            Password = "test999"
        };
        var expected = new LoginResponse()
        {
            Success = false,
            Message = "帳密錯誤!",
            Token = null,
            ExpiredTimeStamp = 0L
        };

        // Act
        var actual = await _target.Handle(request, CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}