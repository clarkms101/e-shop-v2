using System.Linq;

namespace e_shop_api.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}