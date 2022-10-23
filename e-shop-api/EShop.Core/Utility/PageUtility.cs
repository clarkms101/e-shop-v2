using e_shop_api.Core.Dto;
using e_shop_api.Core.Utility.Interface;

namespace e_shop_api.Core.Utility
{
    public class PageUtility : IPageUtility
    {
        public Pagination GetPagination(int totalCount, int page, int pageSize)
        {
            var totalPages = (int) Math.Ceiling(totalCount / (decimal) pageSize);

            return new Pagination()
            {
                PageSize = pageSize,
                CurrentPage = page,
                TotalPages = totalPages,
                HasNextPage = totalPages > page,
                HasPrePage = page > 1
            };
        }
    }
}