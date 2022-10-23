using System.Collections.Generic;
using e_shop_api.Core.Dto;
using e_shop_api.Core.Utility.Dto;

namespace e_shop_api.Applications.SystemCode.Query
{
    public class QuerySystemCodeResponse : BaseResponse
    {
        public List<SelectionItem> Items { get; set; }
    }
}