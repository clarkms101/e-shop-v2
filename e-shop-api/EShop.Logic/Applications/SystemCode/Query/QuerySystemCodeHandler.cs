using e_shop_api.Core.Utility.Dto;
using EShop.Cache.Interface;
using EShop.Entity.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.SystemCode.Query
{
    public class QuerySystemCodeHandler : IRequestHandler<QuerySystemCodeRequest, QuerySystemCodeResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ISystemCodeCacheUtility _systemCodeCacheUtility;
        private readonly ILogger<QuerySystemCodeHandler> _logger;

        public QuerySystemCodeHandler(EShopDbContext eShopDbContext, ISystemCodeCacheUtility systemCodeCacheUtility,
            ILogger<QuerySystemCodeHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _systemCodeCacheUtility = systemCodeCacheUtility;
            _logger = logger;
        }

        public async Task<QuerySystemCodeResponse> Handle(QuerySystemCodeRequest request,
            CancellationToken cancellationToken)
        {
            var selectionItems = await GetSelectionItems(request.Type);

            if (selectionItems.Count != 0)
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

        private async Task<List<SelectionItem>> GetSelectionItems(string itemType)
        {
            var selectionItemsFromCache = _systemCodeCacheUtility.GetSelectionItems(itemType);

            if (selectionItemsFromCache != null)
            {
                return selectionItemsFromCache;
            }

            switch (itemType)
            {
                case "Category":
                    var selectionItems = await _eShopDbContext.Category.Select(s => new SelectionItem()
                    {
                        Text = s.CategoryName,
                        Value = s.Id
                    }).ToListAsync();
                    _systemCodeCacheUtility.AddSelectionItems(itemType, selectionItems);
                    return selectionItems;

                default:
                    return new List<SelectionItem>();
            }
        }
    }
}