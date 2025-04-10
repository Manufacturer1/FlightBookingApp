using BaseEntity.Dtos;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Services.Interfaces;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaggageController : ControllerBase
    {
        private readonly IBaggageService _baggageService;

        public BaggageController(IBaggageService baggageService)
        {
            _baggageService = baggageService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBaggageDto request)
        {
            if(!ModelState.IsValid) 
                return ValidationProblem(ModelState);

            var response = await _baggageService.AddAsync(request);

            if(!response.Flag)
                return BadRequest(response);

            return Ok(response.Message);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _baggageService.DeleteAsync(id);

            if(!response.Flag)
                return BadRequest(response.Message);


            return NoContent();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CreateBaggageDto request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var response = await _baggageService.UpdateAsync(request, id);

            if(!response.Flag)
                return BadRequest(response.Message);    

            return Ok(response.Message);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var baggage = await _baggageService.GetByIdAsync(id);

            if(baggage == null)
                return NotFound();

            return Ok(baggage);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var baggages = await _baggageService.GetAllAsync();

            if (baggages == null)
                return NotFound();

            return Ok(baggages);

        }

    }
}
