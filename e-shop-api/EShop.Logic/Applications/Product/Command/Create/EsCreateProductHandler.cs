using MediatR;
using Microsoft.Extensions.Logging;
using Nest;

namespace EShop.Logic.Applications.Product.Command.Create;

public class EsCreateProductHandler : IRequestHandler<EsCreateProductRequest, EsCreateProductResponse>
{
    private readonly IElasticClient _elasticClient;
    private readonly ILogger<EsCreateProductHandler> _logger;

    public EsCreateProductHandler(IElasticClient elasticClient, ILogger<EsCreateProductHandler> logger)
    {
        _elasticClient = elasticClient;
        _logger = logger;
    }

    public async Task<EsCreateProductResponse> Handle(EsCreateProductRequest request,
        CancellationToken cancellationToken)
    {
        await _elasticClient.IndexAsync(request.Product, idx => idx.Index("product"), cancellationToken);

        return new EsCreateProductResponse()
        {
            Success = true
        };
    }
}