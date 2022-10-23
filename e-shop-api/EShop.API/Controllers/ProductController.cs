using System.Threading.Tasks;
using e_shop_api.ActionFilters;
using EShop.Logic.Applications.Product.Command.Create;
using EShop.Logic.Applications.Product.Command.Delete;
using EShop.Logic.Applications.Product.Command.Update;
using EShop.Logic.Applications.Product.Query;
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
        public async Task<QueryProductResponse> Get([FromRoute] int id)
        {
            var request = new QueryProductRequest
            {
                ProductId = id
            };
            var result = await _mediator.Send(request);
            return result;
        }

        [AdminAuthActionFilter]
        [HttpPost]
        public async Task<CreateProductResponse> Post([FromBody] CreateProductRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [AdminAuthActionFilter]
        [HttpDelete("{id:int}")]
        public async Task<DeleteProductResponse> Delete([FromRoute] int id)
        {
            var request = new DeleteProductRequest()
            {
                ProductId = id
            };
            var result = await _mediator.Send(request);
            return result;
        }

        [AdminAuthActionFilter]
        [HttpPut]
        public async Task<UpdateProductResponse> Put([FromBody] UpdateProductRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }
    }
}