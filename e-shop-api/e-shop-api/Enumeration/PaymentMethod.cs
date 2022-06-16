using System.ComponentModel;

namespace e_shop_api.Enumeration
{
    public enum PaymentMethod
    {
        [Description("信用卡付款")]
        CreditCardPayment,
        [Description("貨到付款")]
        CashOnDelivery
    }
}