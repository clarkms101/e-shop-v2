using e_shop_api.Core.Dto.Product;
using e_shop_api.Core.Enumeration;

namespace e_shop_api.Core.Dto.MQ;

public class EsProductSyncInfo
{
    public DateSyncType SyncType { get; set; }
    public EsProduct? EsProduct { get; set; }
}