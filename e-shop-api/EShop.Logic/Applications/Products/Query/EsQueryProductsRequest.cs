using e_shop_api.Core.Dto;
using MediatR;

namespace EShop.Logic.Applications.Products.Query;

    public class EsQueryProductsRequest : BaseQueryPageRequest, IRequest<EsQueryProductsResponse>
    {
        public string ProductName { get; set; }
    }
