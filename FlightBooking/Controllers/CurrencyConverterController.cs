using BaseEntity.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyConverterController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        [HttpGet("convert")]
        public async Task<IActionResult> ConvertUsdToTarget(
            [FromQuery, Required, Range(0.01, 1000)] decimal amount,
            [FromQuery, Required] CurrencyCode target = CurrencyCode.USD)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _currencyService.ConvertAmount(amount, target);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
