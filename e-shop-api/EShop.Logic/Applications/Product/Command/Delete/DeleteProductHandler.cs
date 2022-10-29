using e_shop_api.Core.Dto.Product;
using e_shop_api.Core.Enumeration;
using EShop.Entity.DataBase;
using EShop.MQ.Producer;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.Product.Command.Delete
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, DeleteProductResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly MqProducer _mqProducer;
        private readonly ILogger<DeleteProductHandler> _logger;

        public DeleteProductHandler(EShopDbContext eShopDbContext, MqProducer mqProducer,
            ILogger<DeleteProductHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _mqProducer = mqProducer;
            _logger = logger;
        }

        public async Task<DeleteProductResponse> Handle(DeleteProductRequest request,
            CancellationToken cancellationToken)
        {
            var selectProduct = await _eShopDbContext.Products.FindAsync(request.ProductId);
            if (selectProduct == null)
            {
                return new DeleteProductResponse()
                {
                    Success = false,
                    Message = "查無該筆資料!"
                };
            }

            _eShopDbContext.Products.Remove(selectProduct);
            await _eShopDbContext.SaveChangesAsync(cancellationToken);

            // 同步到ES
            await _mqProducer.SyncEsProductData(DateSyncType.Delete, new EsProduct()
            {
                Id = selectProduct.Id,
                Category = selectProduct.Category,
                CategoryId = selectProduct.CategoryId,
                Title = selectProduct.Title,
                Content = selectProduct.Content,
                Description = selectProduct.Description,
                ImageUrl = selectProduct.ImageUrl,
                IsEnabled = selectProduct.IsEnabled
            });

            return new DeleteProductResponse()
            {
                Success = true,
                Message = "刪除成功!"
            };
        }
    }
}