using System.Threading.Tasks;
using e_shop_api.Applications.Product.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace e_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:int}")]
        public async Task<JsonResult> Get([FromRoute] int id)
        {
            var request = new QueryProductRequest
            {
                ProductId = id
            };
            var result = await _mediator.Send(request);
            return new JsonResult(result);
        }
    }
}