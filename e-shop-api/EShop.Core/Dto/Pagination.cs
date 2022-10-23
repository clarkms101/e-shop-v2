namespace e_shop_api.Core.Dto
{
    public class Pagination
    {
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool HasPrePage { get; set; }
        public bool HasNextPage { get; set; }
    }
}