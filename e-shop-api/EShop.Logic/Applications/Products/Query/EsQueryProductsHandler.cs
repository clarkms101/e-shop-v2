using e_shop_api.Core.Utility.Interface;
using MediatR;
using Microsoft.Extensions.Logging;
using Nest;

namespace EShop.Logic.Applications.Products.Query;

public class EsQueryProductsHandler : IRequestHandler<EsQueryProductsRequest, EsQueryProductsResponse>
{
    private readonly IElasticClient _elasticClient;
    private readonly ILogger<EsQueryProductsHandler> _logger;
    private readonly IPageUtility _pageUtility;

    public EsQueryProductsHandler(IElasticClient elasticClient, ILogger<EsQueryProductsHandler> logger,
        IPageUtility pageUtility)
    {
        _elasticClient = elasticClient;
        _logger = logger;
        _pageUtility = pageUtility;
    }

    public Task<EsQueryProductsResponse> Handle(EsQueryProductsRequest request, CancellationToken cancellationToken)
    {
        // todo
        throw new NotImplementedException();
    }
}