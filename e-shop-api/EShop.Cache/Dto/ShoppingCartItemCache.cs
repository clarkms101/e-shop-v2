namespace EShop.Cache.Dto
{
    public class ShoppingCartItemCache
    {
        /// <summary>
        /// 購物商品編號 ps: GUID
        /// </summary>
        public string ShoppingItemId { get; set; }

        public int ProductId { get; set; }
        public int Qty { get; set; }
        public decimal Amount { get; set; }
    }
}