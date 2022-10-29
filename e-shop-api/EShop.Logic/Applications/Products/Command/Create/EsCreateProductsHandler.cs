using e_shop_api.Core.Dto.Product;
using MediatR;
using Microsoft.Extensions.Logging;
using Nest;

namespace EShop.Logic.Applications.Products.Command.Create;

public class EsCreateProductsHandler : IRequestHandler<EsCreateProductsRequest, EsCreateProductsResponse>
{
    private readonly IElasticClient _elasticClient;
    private readonly ILogger<EsCreateProductsHandler> _logger;

    public EsCreateProductsHandler(IElasticClient elasticClient, ILogger<EsCreateProductsHandler> logger)
    {
        _elasticClient = elasticClient;
        _logger = logger;
    }

    public async Task<EsCreateProductsResponse> Handle(EsCreateProductsRequest request,
        CancellationToken cancellationToken)
    {
        await _elasticClient.BulkAsync(new BulkRequest()
        {
            Operations = request.Products.Select(t =>
                    new BulkIndexOperation<EsProduct>(t) as IBulkOperation)
                .ToList()
        }, cancellationToken);

        return new EsCreateProductsResponse()
        {
            Success = true
        };
    }
}