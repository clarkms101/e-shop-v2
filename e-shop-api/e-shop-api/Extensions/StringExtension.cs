using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace e_shop_api.Extensions
{
    public static class StringExtension
    {
        public static string Md5Encrypt(this string source)
        {
            return MD5.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(source))
                .Select(s => s.ToString("x2"))
                .StringJoin();
        }

        /// <summary>
        /// 特定日期字串轉成DateTime
        /// </summary>
        /// <param name="source">yyyy-MM-dd</param>
        /// <returns></returns>
        public static DateTime ToDate(this string source)
        {
            return DateTime.ParseExact(source, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}