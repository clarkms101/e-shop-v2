using e_shop_api.Core.Dto.Product;
using e_shop_api.Core.Enumeration;
using EShop.Entity.DataBase;
using EShop.Logic.Applications.SystemCode.Query;
using EShop.MQ.Producer;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.Product.Command.Update
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, UpdateProductResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly QuerySystemCodeHandler _querySystemCodeHandler;
        private readonly MqProducer _mqProducer;
        private readonly ILogger<UpdateProductHandler> _logger;

        public UpdateProductHandler(
            EShopDbContext eShopDbContext,
            QuerySystemCodeHandler querySystemCodeHandler,
            MqProducer mqProducer,
            ILogger<UpdateProductHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _querySystemCodeHandler = querySystemCodeHandler;
            _mqProducer = mqProducer;
            _logger = logger;
        }

        public async Task<UpdateProductResponse> Handle(UpdateProductRequest request,
            CancellationToken cancellationToken)
        {
            var oldProduct = await _eShopDbContext.Products.FindAsync(request.Product.ProductId);
            if (oldProduct == null)
            {
                return new UpdateProductResponse()
                {
                    Success = false,
                    Message = "查無該筆資料!"
                };
            }

            var result = await _querySystemCodeHandler.Handle(new QuerySystemCodeRequest()
            {
                Type = "Category"
            }, cancellationToken);
            var categoryName = result.Items.Single(s => s.Value == request.Product.CategoryId).Text;

            oldProduct.Title = request.Product.Title;
            oldProduct.Category = categoryName;
            oldProduct.CategoryId = request.Product.CategoryId;
            oldProduct.OriginPrice = request.Product.OriginPrice;
            oldProduct.Price = request.Product.Price;
            oldProduct.Unit = request.Product.Unit;
            oldProduct.ImageUrl = request.Product.ImageUrl;
            oldProduct.Description = request.Product.Description;
            oldProduct.Content = request.Product.Content;
            oldProduct.IsEnabled = request.Product.IsEnabled;
            oldProduct.Num = request.Product.Num;
            // system
            oldProduct.LastModifierUserId = request.SystemUserId;
            oldProduct.LastModificationTime = DateTime.Now;

            await _eShopDbContext.SaveChangesAsync(cancellationToken);

            // 同步到ES
            await _mqProducer.SyncEsProductData(DateSyncType.Update, new EsProduct()
            {
                Id = oldProduct.Id,
                Category = oldProduct.Category,
                CategoryId = oldProduct.CategoryId,
                Title = oldProduct.Title,
                Content = oldProduct.Content,
                Description = oldProduct.Description,
                ImageUrl = oldProduct.ImageUrl,
                IsEnabled = oldProduct.IsEnabled
            });
            
            return new UpdateProductResponse()
            {
                Success = true,
                Message = "更新成功!"
            };
        }
    }
}