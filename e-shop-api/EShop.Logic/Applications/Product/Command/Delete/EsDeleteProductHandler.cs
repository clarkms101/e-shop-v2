using e_shop_api.Core.Dto.Product;
using MediatR;
using Microsoft.Extensions.Logging;
using Nest;

namespace EShop.Logic.Applications.Product.Command.Delete;

public class EsDeleteProductHandler : IRequestHandler<EsDeleteProductRequest, EsDeleteProductResponse>
{
    private readonly IElasticClient _elasticClient;
    private readonly ILogger<EsDeleteProductHandler> _logger;

    public EsDeleteProductHandler(IElasticClient elasticClient, ILogger<EsDeleteProductHandler> logger)
    {
        _elasticClient = elasticClient;
        _logger = logger;
    }

    public async Task<EsDeleteProductResponse> Handle(EsDeleteProductRequest request,
        CancellationToken cancellationToken)
    {
        await _elasticClient.DeleteByQueryAsync<EsProduct>(d =>
            d.Query(q => q.Term(p => p.Id, request.ProductId)), cancellationToken);

        return new EsDeleteProductResponse()
        {
            Success = true
        };
    }
}