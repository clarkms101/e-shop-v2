using System.Threading;
using System.Threading.Tasks;
using e_shop_api.DataBase;
using MediatR;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Product.Command.Create
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<CreateProductHandler> _logger;

        public CreateProductHandler(EShopDbContext eShopDbContext, ILogger<CreateProductHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
        }

        public async Task<CreateProductResponse> Handle(CreateProductRequest request,
            CancellationToken cancellationToken)
        {
            var newProduct = new DataBase.Models.Product()
            {
                Title = request.Product.Title,
                Category = request.Product.Category,
                OriginPrice = request.Product.OriginPrice,
                Price = request.Product.Price,
                Unit = request.Product.Unit,
                ImageUrl = request.Product.ImageUrl,
                Description = request.Product.Description,
                Content = request.Product.Content,
                IsEnabled = request.Product.IsEnabled,
                Num = request.Product.Num
            };

            await _eShopDbContext.Products.AddAsync(newProduct, cancellationToken);
            await _eShopDbContext.SaveChangesAsync(cancellationToken);

            return new CreateProductResponse()
            {
                Success = true,
                Message = "新增成功!"
            };
        }
    }
}