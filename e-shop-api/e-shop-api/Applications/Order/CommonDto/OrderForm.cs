namespace e_shop_api.Applications.Order.CommonDto
{
    public class OrderForm
    {
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Message { get; set; }
        public string PaymentMethod { get; set; }
    }
}