using System.Threading;
using System.Threading.Tasks;
using e_shop_api.DataBase;
using MediatR;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Product.Command.Delete
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, DeleteProductResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<DeleteProductHandler> _logger;

        public DeleteProductHandler(EShopDbContext eShopDbContext, ILogger<DeleteProductHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
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

            return new DeleteProductResponse()
            {
                Success = true,
                Message = "刪除成功!"
            };
        }
    }
}