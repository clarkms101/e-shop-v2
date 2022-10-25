namespace e_shop_api.Core.Dto.Cart
{
    public class ShoppingProduct
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public string Unit { get; set; }
    }
}