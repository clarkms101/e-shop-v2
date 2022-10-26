using e_shop_api.Core.Utility.Interface;
using EShop.Logic.Applications.Product.CommonDto;
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

    public async Task<EsQueryProductsResponse> Handle(EsQueryProductsRequest request,
        CancellationToken cancellationToken)
    {
        var response = _elasticClient.Search<EsProduct>(s => s
                .From(0)
                .Size(10)
                .Query(q =>
                    q.QueryString(qs =>
                        qs.Fields(p => p.Field(product => product.Title))
                            .Query(request.ProductName))
                )
                .Sort(q => q.Ascending(u => u.Id)))
            .Hits
            .Select(h => h.Source)
            .ToList();

        var totalCount = _elasticClient.Search<EsProduct>(s => s
                .Query(q =>
                    q.QueryString(qs =>
                        qs.Fields(p => p.Field(product => product.Title))
                            .Query(request.ProductName))
                )
                .Sort(q => q.Ascending(u => u.Id)))
            .Hits
            .Count();

        return new EsQueryProductsResponse()
        {
            Success = true,
            Message = "查詢成功",
            Products = response,
            Pagination = _pageUtility.GetPagination(totalCount, request.Page, request.PageSize)
        };
    }
}