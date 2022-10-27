using System.Threading.Tasks;
using EShop.Logic.Applications.Product.Command.Create;
using EShop.Logic.Applications.Product.Command.Delete;
using EShop.Logic.Applications.Product.Command.Update;
using EShop.Logic.Applications.Products.Command.Create;
using EShop.Logic.Applications.Products.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace e_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EsProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EsProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // /// <summary>
        // /// 測試用
        // /// </summary>
        // /// <param name="request"></param>
        // /// <returns></returns>
        // [HttpPost]
        // public async Task<EsCreateProductResponse> Create([FromBody] EsCreateProductRequest request)
        // {
        //     var result = await _mediator.Send(request);
        //     return result;
        // }
        //
        // /// <summary>
        // /// 測試用
        // /// </summary>
        // /// <param name="request"></param>
        // /// <returns></returns>
        // [HttpPost]
        // public async Task<EsCreateProductsResponse> CreateList([FromBody] EsCreateProductsRequest request)
        // {
        //     var result = await _mediator.Send(request);
        //     return result;
        // }
        //
        // /// <summary>
        // /// 測試用
        // /// </summary>
        // /// <param name="request"></param>
        // /// <returns></returns>
        // [HttpPost]
        // public async Task<EsUpdateProductResponse> Update([FromBody] EsUpdateProductRequest request)
        // {
        //     var result = await _mediator.Send(request);
        //     return result;
        // }
        //
        // /// <summary>
        // /// 測試用
        // /// </summary>
        // /// <param name="request"></param>
        // /// <returns></returns>
        // [HttpPost]
        // public async Task<EsDeleteProductResponse> Delete([FromBody] EsDeleteProductRequest request)
        // {
        //     var result = await _mediator.Send(request);
        //     return result;
        // }

        [HttpPost]
        public async Task<EsQueryProductsResponse> GetEsProductList([FromBody] EsQueryProductsRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }
    }
}