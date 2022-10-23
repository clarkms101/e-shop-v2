using e_shop_api.Core.Dto;

namespace e_shop_api.Core.Utility.Interface
{
    public interface IPageUtility
    {
        Pagination GetPagination(int totalCount, int page, int pageSize);
    }
}