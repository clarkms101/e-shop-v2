using e_shop_api.Core.Dto;

namespace EShop.Logic.Applications.Admin.Command.Login
{
    public class LoginResponse : BaseResponse
    {
        public string Token { get; set; }
        public long ExpiredTimeStamp { get; set; }
    }
}