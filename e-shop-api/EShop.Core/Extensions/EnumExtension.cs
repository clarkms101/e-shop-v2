using System.ComponentModel;

namespace e_shop_api.Core.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescriptionText(this Enum source)
        {
            // 取得Enum欄位
            var fi = source.GetType().GetField(source.ToString());

            // 取得Enum欄位的所有attr
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            // 有設定Description attr的再顯示裡面的文字，沒有則用原本的Enum轉成文字
            return attributes.Length > 0 ? attributes[0].Description : source.ToString();
        }
    }
}