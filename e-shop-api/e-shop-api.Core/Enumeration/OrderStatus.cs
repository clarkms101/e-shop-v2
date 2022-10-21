using System.ComponentModel;

namespace e_shop_api.Core.Enumeration
{
    public enum OrderStatus
    {
        [Description("訂單已建立")]
        Created,
        [Description("交易完成")]
        Finished,
        [Description("交易取消")]
        Cancel,
        [Description("交易退款")]
        Refund
    }
}