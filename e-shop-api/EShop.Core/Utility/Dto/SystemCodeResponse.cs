using e_shop_api.Core.Dto;

namespace e_shop_api.Core.Utility.Dto
{
    public class SystemCodeResponse:BaseResponse
    {
        public List<SelectionItem> Items { get; set; }
    }
}