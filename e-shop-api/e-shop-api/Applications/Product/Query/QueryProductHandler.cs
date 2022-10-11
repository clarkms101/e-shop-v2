using System.Threading;
using System.Threading.Tasks;
using e_shop_api.DataBase;
using MediatR;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Product.Query
{
    public class QueryProductHandler : IRequestHandler<QueryProductRequest, QueryProductResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<QueryProductHandler> _logger;

        public QueryProductHandler(EShopDbContext eShopDbContext, ILogger<QueryProductHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
        }

        public async Task<QueryProductResponse> Handle(QueryProductRequest request, CancellationToken cancellationToken)
        {
            var selectProduct = await _eShopDbContext.Products.FindAsync(request.ProductId);
            if (selectProduct == null)
            {
                return new QueryProductResponse()
                {
                    Success = false,
                    Message = "查無該筆資料!"
                };
            }

            return new QueryProductResponse()
            {
                Success = true,
                Message = "查詢成功!",
                Product = new CommonDto.Product()
                {
                    CategoryId = selectProduct.CategoryId,
                    Category = selectProduct.Category,
                    Content = selectProduct.Content,
                    Description = selectProduct.Description,
                    ImageUrl = selectProduct.ImageUrl,
                    IsEnabled = selectProduct.IsEnabled,
                    Num = selectProduct.Num,
                    OriginPrice = selectProduct.OriginPrice,
                    Price = selectProduct.Price,
                    ProductId = selectProduct.Id,
                    Title = selectProduct.Title,
                    Unit = selectProduct.Unit
                }
            };
        }
    }
}