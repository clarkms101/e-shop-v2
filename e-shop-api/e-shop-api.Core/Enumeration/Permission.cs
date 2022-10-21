namespace e_shop_api.Core.Enumeration
{
    public enum Permission
    {
        /// <summary>
        /// (一般開放使用)僅能唯讀後台資料
        /// </summary>
       Public,
        /// <summary>
        /// (管理者)可以異動後台資料
        /// </summary>
       Owner
    }
}