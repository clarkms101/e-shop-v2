using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.DataBase;
using e_shop_api.Utility.Dto;
using MediatR;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.SystemCode.Query
{
    public class QuerySystemCodeHandler : IRequestHandler<QuerySystemCodeRequest, QuerySystemCodeResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<QuerySystemCodeHandler> _logger;

        public QuerySystemCodeHandler(EShopDbContext eShopDbContext, ILogger<QuerySystemCodeHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
        }

        public async Task<QuerySystemCodeResponse> Handle(QuerySystemCodeRequest request,
            CancellationToken cancellationToken)
        {
            switch (request.Type)
            {
                case "Category":
                    // todo 放入快取
                    return new QuerySystemCodeResponse()
                    {
                        Success = true,
                        Items = _eShopDbContext.Category.Select(s => new SelectionItem()
                        {
                            Text = s.CategoryName,
                            Value = s.Id
                        }).ToList()
                    };
                default:
                    return new QuerySystemCodeResponse();
            }
        }
    }
}