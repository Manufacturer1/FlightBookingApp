using BaseEntity.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Services.Interfaces;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(CreateDiscountDto request)
        {
            var response = await _discountService.AddAsync(request);
            if(!response.Flag)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _discountService.DeleteAsync(id);

            if (!response.Flag)
                return BadRequest(response.Message);

            return NoContent();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAsync(UpdateDiscountDto request, int id)
        {
            var response = await _discountService.UpdateAsync(request, id);
            if (!response.Flag)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var discounts = await _discountService.GetAllAsync();

            return Ok(discounts);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var discount = await _discountService.GetByIdAsync(id);

            return Ok(discount);
        }
    }
}
