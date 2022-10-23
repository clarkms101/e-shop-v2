using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Core.Utility.Dto;
using EShop.Cache.Interface;
using EShop.Entity.DataBase;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace e_shop_api.Applications.SystemCode.Query
{
    public class QuerySystemCodeHandler : IRequestHandler<QuerySystemCodeRequest, QuerySystemCodeResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly IMemoryCacheUtility _memoryCacheUtility;
        private readonly ILogger<QuerySystemCodeHandler> _logger;

        public QuerySystemCodeHandler(EShopDbContext eShopDbContext, IMemoryCacheUtility memoryCacheUtility,
            ILogger<QuerySystemCodeHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _memoryCacheUtility = memoryCacheUtility;
            _logger = logger;
        }

        public async Task<QuerySystemCodeResponse> Handle(QuerySystemCodeRequest request,
            CancellationToken cancellationToken)
        {
            var selectionItems = GetSelectionItems(request.Type);

            if (selectionItems != null && selectionItems.Count != 0)
            {
                return new QuerySystemCodeResponse()
                {
                    Success = true,
                    Items = selectionItems
                };
            }

            return new QuerySystemCodeResponse()
            {
                Success = false,
                Message = "沒有資料"
            };
        }

        private List<SelectionItem> GetSelectionItems(string itemType)
        {
            var selectionJsonString = _memoryCacheUtility.Get<string>(itemType);

            // 快取有資料直接回傳
            if (string.IsNullOrWhiteSpace(selectionJsonString) == false)
            {
                var selectionItems = JsonConvert.DeserializeObject<List<SelectionItem>>(selectionJsonString);
                return selectionItems;
            }

            // 沒有資料則加入快取
            switch (itemType)
            {
                case "Category":
                    var selectionItems = _eShopDbContext.Category.Select(s => new SelectionItem()
                    {
                        Text = s.CategoryName,
                        Value = s.Id
                    }).ToList();
                    _memoryCacheUtility.Add(new CacheItem(itemType, JsonConvert.SerializeObject(selectionItems)),
                        new CacheItemPolicy());
                    return selectionItems;
                default:
                    return new List<SelectionItem>();
            }
        }
    }
}