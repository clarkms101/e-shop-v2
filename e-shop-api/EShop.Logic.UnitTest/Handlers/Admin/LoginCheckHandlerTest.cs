using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Core.Utility.Dto;
using EShop.Cache.Interface;
using EShop.Logic.Applications.Admin.Command.LoginCheck;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace e_shop_api_unit_test.Handlers.Admin;

public class LoginCheckHandlerTest
{
        private readonly LoginCheckHandler _target;

        /// <summary>
        /// 測試的前置作業
        /// </summary>
        public LoginCheckHandlerTest()
        {
            // cache
            var adminCacheInfo = new AdminInfo()
            {
                Account = "Clark",
                ApiAccessKey = "TestKey",
                Device = "test",
                ExpiredTimeStamp = 12345
            };
            var adminCacheInfoJsonString = JsonConvert.SerializeObject(adminCacheInfo);

            var fakeMemoryCacheUtility = Substitute.For<IMemoryCacheUtility>();
            fakeMemoryCacheUtility.Get<string>("TestKey").Returns(adminCacheInfoJsonString);
            
            // log
            var fakeLog = Substitute.For<ILogger<LoginCheckHandler>>();
            
            _target = new LoginCheckHandler(fakeMemoryCacheUtility, fakeLog);
        }

        [Fact]
        public async Task 使用存在的ApiAccessKey檢查登入狀態_返回成功和登入者上線狀態()
        {
            // Arrange
            var request = new LoginCheckRequest()
            {
                ApiAccessKey = "TestKey"
            };
            var expected = new LoginCheckResponse()
            {
                    Success = true,
                    Message = "Online",
                    Account = "Clark",
                    ExpiredTimeStamp = 12345
            };

            // Act
            var actual = await _target.Handle(request, CancellationToken.None);
            
            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task 使用不存在的ApiAccessKey檢查登入狀態_返回失敗和下線狀態()
        {
            // Arrange
            var request = new LoginCheckRequest()
            {
                ApiAccessKey = "ErrorKey"
            };
            var expected = new LoginCheckResponse()
            {
                Success = false,
                Message = "Offline"
            };

            // Act
            var actual = await _target.Handle(request, CancellationToken.None);
            
            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
}