using e_shop_api.Core.Dto;
using e_shop_api.Core.Extensions;
using e_shop_api.Core.Utility.Interface;
using EShop.Entity.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.Product.Query
{
    public class QueryProductsHandler : IRequestHandler<QueryProductsRequest, QueryProductsResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<QueryProductsHandler> _logger;
        private readonly IPageUtility _pageUtility;

        public QueryProductsHandler(EShopDbContext eShopDbContext, ILogger<QueryProductsHandler> logger,
            IPageUtility pageUtility)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
            _pageUtility = pageUtility;
        }

        public async Task<QueryProductsResponse> Handle(QueryProductsRequest request,
            CancellationToken cancellationToken)
        {
            // 預設查詢
            if (request.Category == "default")
            {
                request.Category = "金牌";
            }

            var totalCount = await GetTotalCount(request, cancellationToken);
            if (totalCount == 0)
            {
                return new QueryProductsResponse()
                {
                    Success = true,
                    Message = "沒有資料",
                    Products = new List<CommonDto.Product>(),
                    Pagination = new Pagination()
                };
            }

            var productsQuery = _eShopDbContext.Products.AsQueryable();

            // 過濾查詢
            productsQuery = ProductsFilter(request, productsQuery);

            var products = await productsQuery
                .OrderBy(s => s.Id)
                .Page(request.Page, request.PageSize)
                .Select(s => new CommonDto.Product()
                {
                    ProductId = s.Id,
                    Title = s.Title,
                    CategoryId = s.CategoryId,
                    Category = s.Category,
                    Content = s.Content,
                    Description = s.Description,
                    IsEnabled = s.IsEnabled,
                    OriginPrice = s.OriginPrice,
                    Price = s.Price,
                    ImageUrl = s.ImageUrl,
                    Unit = s.Unit,
                    Num = s.Num
                })
                .ToListAsync(cancellationToken: cancellationToken);

            return new QueryProductsResponse()
            {
                Success = true,
                Message = "查詢成功",
                Products = products,
                Pagination = _pageUtility.GetPagination(totalCount, request.Page, request.PageSize)
            };
        }

        private async Task<int> GetTotalCount(QueryProductsRequest request, CancellationToken cancellationToken)
        {
            var products = _eShopDbContext.Products.AsQueryable();
            products = ProductsFilter(request, products);

            var totalCount = await products.CountAsync(cancellationToken: cancellationToken);
            return totalCount;
        }

        private static IQueryable<EShop.Entity.DataBase.Models.Product> ProductsFilter(QueryProductsRequest request,
            IQueryable<EShop.Entity.DataBase.Models.Product> products)
        {
            if (request.CategoryId != null)
            {
                products = products.Where(s => s.CategoryId == request.CategoryId);
            }

            if (string.IsNullOrWhiteSpace(request.Category) == false)
            {
                products = products.Where(s => s.Category == request.Category);
            }

            if (string.IsNullOrWhiteSpace(request.ProductName) == false)
            {
                products = products.Where(s => s.Title.Contains(request.ProductName));
            }

            return products;
        }
    }
}