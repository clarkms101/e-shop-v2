using e_shop_api.Applications;

namespace e_shop_api.Utility.Interface
{
    public interface IPageUtility
    {
        Pagination GetPagination(int totalCount, int page, int pageSize);
    }
}