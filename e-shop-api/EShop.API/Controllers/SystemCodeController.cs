using System.Collections.Generic;
using System.Threading.Tasks;
using e_shop_api.Applications.SystemCode.Query;
using e_shop_api.Core.Utility;
using e_shop_api.Core.Utility.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace e_shop_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemCodeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SystemCodeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<SystemCodeResponse> Category()
        {
            var result = await _mediator.Send(new QuerySystemCodeRequest()
            {
                Type = "Category"
            });

            return new SystemCodeResponse()
            {
                Success = result.Success,
                Items = result.Items
            };
        }

        [HttpGet]
        public async Task<SystemCodeResponse> PaymentMethod()
        {
            return new SystemCodeResponse()
            {
                Success = true,
                Items = Selection.GetPaymentMethod()
            };
        }

        [HttpGet]
        public async Task<SystemCodeResponse> Country()
        {
            return new SystemCodeResponse()
            {
                Success = true,
                Items = Selection.GetCountry()
            };
        }

        [HttpGet("{countryId:int}")]
        public async Task<SystemCodeResponse> City([FromRoute] int countryId)
        {
            return new SystemCodeResponse()
            {
                Success = true,
                Items = Selection.GetCity(countryId)
            };
        }
    }
}