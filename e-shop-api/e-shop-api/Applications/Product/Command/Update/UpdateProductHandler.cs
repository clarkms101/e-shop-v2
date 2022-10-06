using System;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.DataBase;
using MediatR;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Product.Command.Update
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, UpdateProductResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<UpdateProductHandler> _logger;

        public UpdateProductHandler(EShopDbContext eShopDbContext, ILogger<UpdateProductHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
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

            oldProduct.Title = request.Product.Title;
            oldProduct.Category = request.Product.Category;
            oldProduct.OriginPrice = request.Product.OriginPrice;
            oldProduct.Price = request.Product.Price;
            oldProduct.Unit = request.Product.Unit;
            oldProduct.ImageUrl = request.Product.ImageUrl;
            oldProduct.Description = request.Product.Description;
            oldProduct.Content = request.Product.Content;
            oldProduct.IsEnabled = request.Product.IsEnabled;
            oldProduct.Num = request.Product.Num;
            oldProduct.LastModificationTime = DateTime.Now;

            await _eShopDbContext.SaveChangesAsync(cancellationToken);

            return new UpdateProductResponse()
            {
                Success = true,
                Message = "更新成功!"
            };
        }
    }
}