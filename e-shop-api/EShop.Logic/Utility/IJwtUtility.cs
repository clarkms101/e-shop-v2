using e_shop_api.Core.Utility.Dto;

namespace EShop.Logic.Utility
{
    public interface IJwtUtility
    {
        long GetExpiredTimeStamp();

        /// <summary>
        /// 將使用者資訊放到JWT裡面,並產生Token
        /// </summary>
        /// <param name="adminInfo"></param>
        /// <returns></returns>
        string GenerateAdminToken(AdminInfo adminInfo);

        /// <summary>
        /// 從JWT裡面解析出使用者資訊
        /// </summary>
        /// <returns></returns>
        AdminInfo GetAdminInfo();

        /// <summary>
        /// 取得JWT保存於快取的Key
        /// </summary>
        /// <param name="apiAccessKey"></param>
        /// <returns></returns>
        string GetApiAccessKey(long apiAccessKey);
    }
}