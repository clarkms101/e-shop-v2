using e_shop_api.Core.Utility.Dto;

namespace EShop.Cache.Interface;

public interface ISystemCodeCacheUtility
{
    void AddSelectionItems(string itemType, List<SelectionItem> items);
    List<SelectionItem>? GetSelectionItems(string itemType);
}