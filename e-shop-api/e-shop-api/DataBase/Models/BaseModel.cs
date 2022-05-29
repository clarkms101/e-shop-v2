using System;

namespace e_shop_api.DataBase.Models
{
    public class BaseModel
    {
        /// <summary>
        /// 建立者編號
        /// ps:目前沒有帳號登入機制，一律先填Null
        /// </summary>
        public long? CreatorUserId { get; set; }
        
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreationTime => DateTime.Now;
        
        /// <summary>
        /// 最後異動者編號
        /// ps:目前沒有帳號登入機制，一律先填Null
        /// </summary>
        public long? LastModifierUserId { get; set; }
        
        /// <summary>
        /// 最後異動時間
        /// </summary>
        public DateTime LastModificationTime => DateTime.Now;
    }
}