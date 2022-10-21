namespace e_shop_api.Applications.Cart.CommonDto
{
    public class ShoppingProduct
    {
        public string Category { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsEnabled { get; set; }
        public decimal OriginPrice { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public string Unit { get; set; }
        public int Num { get; set; }
    }
}