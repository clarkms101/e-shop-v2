using System.Threading.Tasks;
using e_shop_api.Applications.Product.Query;
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
        public async Task<JsonResult> Post([FromBody] QueryProductsRequest request)
        {
            var result = await _mediator.Send(request);
            return new JsonResult(result);
        }
    }
}