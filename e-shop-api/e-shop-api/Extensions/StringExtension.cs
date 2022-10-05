using System;
using System.Collections.Generic;
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

        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            if (str == null)
                return (string)null;
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            // if (((ICollection<string>) postFixes).IsNullOrEmpty<string>())
            //     return str;
            foreach (string postFix in postFixes)
            {
                if (str.EndsWith(postFix))
                    return str.Left(str.Length - postFix.Length);
            }

            return str;
        }

        public static string Left(this string str, int len)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            if (str.Length < len)
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            return str.Substring(0, len);
        }

        public static T FromStringToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}