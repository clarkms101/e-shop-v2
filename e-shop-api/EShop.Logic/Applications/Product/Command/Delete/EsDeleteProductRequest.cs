using e_shop_api.Core.Dto;
using MediatR;

namespace EShop.Logic.Applications.Product.Command.Delete;

public class EsDeleteProductRequest : BaseCommandRequest, IRequest<EsDeleteProductResponse>
{
    public int ProductId { get; set; }
}