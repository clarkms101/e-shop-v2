using System.Collections.Generic;
using e_shop_api.Utility.Dto;

namespace e_shop_api.Applications.SystemCode.Query
{
    public class QuerySystemCodeResponse : BaseResponse
    {
        public List<SelectionItem> Items { get; set; }
    }
}