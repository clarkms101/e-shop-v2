using e_shop_api.Core.Dto.Product;
using e_shop_api.Core.Enumeration;
using EShop.Entity.DataBase;
using EShop.Logic.Applications.SystemCode.Query;
using EShop.MQ.Producer;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.Product.Command.Create
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly QuerySystemCodeHandler _querySystemCodeHandler;
        private readonly MqProducer _mqProducer;
        private readonly ILogger<CreateProductHandler> _logger;

        public CreateProductHandler(
            EShopDbContext eShopDbContext,
            QuerySystemCodeHandler querySystemCodeHandler,
            MqProducer mqProducer,
            ILogger<CreateProductHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _querySystemCodeHandler = querySystemCodeHandler;
            _mqProducer = mqProducer;
            _logger = logger;
        }

        public async Task<CreateProductResponse> Handle(CreateProductRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _querySystemCodeHandler.Handle(new QuerySystemCodeRequest()
            {
                Type = "Category"
            }, cancellationToken);
            var categoryName = result.Items.Single(s => s.Value == request.Product.CategoryId).Text;
            var newProduct = new EShop.Entity.DataBase.Models.Product()
            {
                Title = request.Product.Title,
                Category = categoryName,
                CategoryId = request.Product.CategoryId,
                OriginPrice = request.Product.OriginPrice,
                Price = request.Product.Price,
                Unit = request.Product.Unit,
                ImageUrl = request.Product.ImageUrl,
                Description = request.Product.Description,
                Content = request.Product.Content,
                IsEnabled = request.Product.IsEnabled,
                Num = request.Product.Num,
                // system
                CreatorUserId = request.SystemUserId,
                CreationTime = DateTime.Now
            };

            await _eShopDbContext.Products.AddAsync(newProduct, cancellationToken);
            await _eShopDbContext.SaveChangesAsync(cancellationToken);

            // 同步到ES
            await _mqProducer.SyncEsProductData(DateSyncType.Create, new EsProduct()
            {
                Id = newProduct.Id,
                Category = newProduct.Category,
                CategoryId = newProduct.CategoryId,
                Title = newProduct.Title,
                Content = newProduct.Content,
                Description = newProduct.Description,
                ImageUrl = newProduct.ImageUrl,
                IsEnabled = newProduct.IsEnabled
            });

            return new CreateProductResponse()
            {
                Success = true,
                Message = "新增成功!"
            };
        }
    }
}