using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;

namespace EShop.Logic.Applications.Product.Command.Create;

public class EsCreateProductHandler : IRequestHandler<EsCreateProductRequest, EsCreateProductResponse>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<EsCreateProductHandler> _logger;

    public EsCreateProductHandler(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<EsCreateProductHandler> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task<EsCreateProductResponse> Handle(EsCreateProductRequest request,
        CancellationToken cancellationToken)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var elasticClient = serviceScope.ServiceProvider.GetRequiredService<IElasticClient>();
        await elasticClient.IndexAsync(request.Product, idx => idx.Index("product"), cancellationToken);

        return new EsCreateProductResponse()
        {
            Success = true
        };
    }
}