using e_shop_api.Core.Dto.Product;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;

namespace EShop.Logic.Applications.Product.Command.Delete;

public class EsDeleteProductHandler : IRequestHandler<EsDeleteProductRequest, EsDeleteProductResponse>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<EsDeleteProductHandler> _logger;

    public EsDeleteProductHandler(IServiceScopeFactory serviceScopeFactory, ILogger<EsDeleteProductHandler> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task<EsDeleteProductResponse> Handle(EsDeleteProductRequest request,
        CancellationToken cancellationToken)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var elasticClient = serviceScope.ServiceProvider.GetRequiredService<IElasticClient>();
        await elasticClient.DeleteByQueryAsync<EsProduct>(d =>
            d.Query(q => q.Term(p => p.Id, request.ProductId)), cancellationToken);

        return new EsDeleteProductResponse()
        {
            Success = true
        };
    }
}