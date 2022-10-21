using System.Collections.Generic;
using e_shop_api.Applications;

namespace e_shop_api.Utility.Dto
{
    public class SystemCodeResponse:BaseResponse
    {
        public List<SelectionItem> Items { get; set; }
    }
}