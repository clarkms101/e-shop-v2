namespace e_shop_api.Applications.Admin.Command.Login
{
    public class LoginResponse : BaseResponse
    {
        public string Token { get; set; }
        public long ExpiredTimeStamp { get; set; }
    }
}