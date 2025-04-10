using BaseEntity.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Services.Interfaces;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirlineController : ControllerBase
    {
        private readonly IAirlineService _airlineService;

        public AirlineController(IAirlineService airlineService)
        {
            _airlineService = airlineService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAirlineAsync([FromForm] CreateAirlineDto request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _airlineService.AddAsync(request);

            if (!result.Flag)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> RemoveAirline(int id)
        {
            var result = await _airlineService.DeleteAsync(id);

            if (!result.Flag)
                return BadRequest(result.Message);


            return NoContent();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateAirlineDto request)
        {
            if(!ModelState.IsValid) return ValidationProblem(ModelState);

            var response = await _airlineService.UpdateAsync(request);

            if(!response.Flag)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var airline = await _airlineService.GetByIdAsync(id);

            if(airline == null)
                return NotFound();

            return Ok(airline);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _airlineService.GetAllAsync());
        }
    }
}
