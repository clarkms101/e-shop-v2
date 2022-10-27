using EShop.Logic.Applications.Product.CommonDto;
using MediatR;
using Microsoft.Extensions.Logging;
using Nest;

namespace EShop.Logic.Applications.Product.Command.Update;

public class EsUpdateProductHandler : IRequestHandler<EsUpdateProductRequest, EsUpdateProductResponse>
{
    private readonly IElasticClient _elasticClient;
    private readonly ILogger<EsUpdateProductHandler> _logger;

    public EsUpdateProductHandler(IElasticClient elasticClient, ILogger<EsUpdateProductHandler> logger)
    {
        _elasticClient = elasticClient;
        _logger = logger;
    }

    public async Task<EsUpdateProductResponse> Handle(EsUpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var docId = _elasticClient.Search<EsProduct>(s => s
                .Query(q =>
                    q.Term(t => t.Id, request.Product.Id)))
            .Hits.Select(h => new
            {
                docId = h.Id,
            }).First().docId;

        _elasticClient.Update(DocumentPath<EsProduct>.Id(docId),
            u => u.Index("product").DocAsUpsert(true).Doc(request.Product));

        return new EsUpdateProductResponse()
        {
            Success = true
        };
    }
}