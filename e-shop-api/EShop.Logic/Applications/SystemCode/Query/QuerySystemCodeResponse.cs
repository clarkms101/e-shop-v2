using e_shop_api.Core.Dto;
using e_shop_api.Core.Utility.Dto;

namespace EShop.Logic.Applications.SystemCode.Query
{
    public class QuerySystemCodeResponse : BaseResponse
    {
        public List<SelectionItem> Items { get; set; }
    }
}