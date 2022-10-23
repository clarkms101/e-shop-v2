using e_shop_api.Core.Dto;

namespace e_shop_api.Applications.Admin.Command.LoginCheck
{
    public class LoginCheckResponse : BaseResponse
    {
        public string Account { get; set; }
        public long ExpiredTimeStamp { get; set; }
    }
}