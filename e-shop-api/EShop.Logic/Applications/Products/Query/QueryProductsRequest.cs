using e_shop_api.Core.Dto;
using MediatR;

namespace EShop.Logic.Applications.Products.Query
{
    public class QueryProductsRequest : BaseQueryPageRequest, IRequest<QueryProductsResponse>
    {
        public int? CategoryId { get; set; }
        public string? Category { get; set; }
        public string? ProductName { get; set; }
    }
}