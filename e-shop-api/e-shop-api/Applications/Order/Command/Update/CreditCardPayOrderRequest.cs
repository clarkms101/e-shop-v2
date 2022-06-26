using MediatR;

namespace e_shop_api.Applications.Order.Command.Update
{
    public class CreditCardPayOrderRequest : IRequest<CreditCardPayOrderResponse>
    {
        public int OrderId { get; set; }
        public string CardUserName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiration { get; set; }
        public string CardCvc { get; set; }
    }
}