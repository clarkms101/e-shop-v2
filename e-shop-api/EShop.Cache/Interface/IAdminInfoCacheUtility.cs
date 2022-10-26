using e_shop_api.Core.Utility.Dto;

namespace EShop.Cache.Interface;

public interface IAdminInfoCacheUtility
{
    void AddAdminInfo(string apiAccessKey, AdminInfo adminInfo);
    void RemoveAdminInfo(string apiAccessKey);
    AdminInfo? GetAdminInfo(string apiAccessKey);
}