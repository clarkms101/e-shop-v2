using e_shop_api.Core.Dto.Product;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;

namespace EShop.Logic.Applications.Product.Command.Update;

public class EsUpdateProductHandler : IRequestHandler<EsUpdateProductRequest, EsUpdateProductResponse>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<EsUpdateProductHandler> _logger;

    public EsUpdateProductHandler(IServiceScopeFactory serviceScopeFactory, ILogger<EsUpdateProductHandler> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task<EsUpdateProductResponse> Handle(EsUpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var elasticClient = serviceScope.ServiceProvider.GetRequiredService<IElasticClient>();

        var docId = elasticClient.Search<EsProduct>(s => s
                .Query(q =>
                    q.Term(t => t.Id, request.Product.Id)))
            .Hits.Select(h => new
            {
                docId = h.Id,
            }).First().docId;

        elasticClient.Update(DocumentPath<EsProduct>.Id(docId),
            u => u.Index("product").DocAsUpsert(true).Doc(request.Product));

        return new EsUpdateProductResponse()
        {
            Success = true
        };
    }
}