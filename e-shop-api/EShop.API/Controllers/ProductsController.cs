using System.Threading.Tasks;
using EShop.Logic.Applications.Product.Query;
using EShop.Logic.Applications.Products.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace e_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<QueryProductsResponse> Post([FromBody] QueryProductsRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }
    }
}