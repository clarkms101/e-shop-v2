using e_shop_api.Utility;
using Microsoft.AspNetCore.Mvc;

namespace e_shop_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemCodeController : ControllerBase
    {
        [HttpGet]
        public JsonResult PaymentMethod()
        {
            return new JsonResult(Selection.GetPaymentMethod());
        }

        [HttpGet]
        public JsonResult Country()
        {
            return new JsonResult(Selection.GetCountry());
        }

        [HttpGet("{countryId:int}")]
        public JsonResult City([FromRoute] int countryId)
        {
            return new JsonResult(Selection.GetCity(countryId));
        }
    }
}