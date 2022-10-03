using System.Collections.Generic;
using System.Threading.Tasks;
using e_shop_api.Utility;
using e_shop_api.Utility.Dto;
using Microsoft.AspNetCore.Mvc;

namespace e_shop_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemCodeController : ControllerBase
    {
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