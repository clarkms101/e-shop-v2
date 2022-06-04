using System.Collections.Generic;

namespace e_shop_api.Extensions
{
    public static class IEnumerableExtension
    {
        public static string StringJoin(this IEnumerable<string> collection, string separator)
        {
            return string.Join(separator, collection);
        }

        public static string StringJoin(this IEnumerable<string> collection)
        {
            return string.Join(string.Empty, collection);
        }
    }
}