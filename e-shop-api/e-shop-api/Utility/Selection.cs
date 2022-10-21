using System.Collections.Generic;
using System.Linq;
using e_shop_api.Core.Enumeration;
using e_shop_api.Core.Extensions;
using e_shop_api.Extensions;
using e_shop_api.Utility.Dto;

namespace e_shop_api.Utility
{
    public class Selection
    {
        public static List<SelectionItem> GetCountry()
        {
            return new List<SelectionItem>()
            {
                new SelectionItem() { Value = 1, Text = "台灣" },
                new SelectionItem() { Value = 2, Text = "外島" }
            };
        }

        public static List<SelectionItem> GetPaymentMethod()
        {
            return new List<SelectionItem>()
            {
                new SelectionItem() { Value = 1, Text = "全部" },
                new SelectionItem() { Value = 2, Text = PaymentMethod.CashOnDelivery.GetDescriptionText() },
                new SelectionItem() { Value = 3, Text = PaymentMethod.CreditCardPayment.GetDescriptionText() }
            };
        }

        public static List<SelectionItem> GetCity(int countryId)
        {
            var cityList = new List<CityInfo>()
            {
                new CityInfo()
                {
                    CityId = 1,
                    CityName = "臺北市",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 2,
                    CityName = "新北市",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 3,
                    CityName = "桃園市",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 4,
                    CityName = "臺中市",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 5,
                    CityName = "臺南市",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 6,
                    CityName = "高雄市",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 7,
                    CityName = "新竹縣",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 8,
                    CityName = "苗栗縣",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 9,
                    CityName = "彰化縣",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 10,
                    CityName = "南投縣",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 11,
                    CityName = "雲林縣",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 12,
                    CityName = "嘉義縣",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 13,
                    CityName = "屏東縣",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 14,
                    CityName = "宜蘭縣",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 15,
                    CityName = "花蓮縣",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 16,
                    CityName = "臺東縣",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 17,
                    CityName = "基隆市",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 18,
                    CityName = "新竹市",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 19,
                    CityName = "嘉義市",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 20,
                    CityName = "澎湖縣",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 21,
                    CityName = "金門縣",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 22,
                    CityName = "連江縣",
                    CountryId = 1
                },
                new CityInfo()
                {
                    CityId = 23,
                    CityName = "香港",
                    CountryId = 2
                },
                new CityInfo()
                {
                    CityId = 24,
                    CityName = "馬來西亞",
                    CountryId = 2
                }
            };

            return cityList
                .Where(n => n.CountryId == countryId)
                .Select(n => new SelectionItem
                {
                    Value = n.CityId,
                    Text = n.CityName
                }).ToList();
        }
    }
}