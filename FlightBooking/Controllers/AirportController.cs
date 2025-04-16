using BaseEntity.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Services.Interfaces;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly IAirportService _airportService;

        public AirportController(IAirportService airportService)
        {
            _airportService = airportService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> CreateAsync(CreateAirportDto request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var response = await _airportService.AddAsync(request);

            if(!response.Flag)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id <= 0)
                return BadRequest("The id cannot be less or equal to zero");

            var response = await _airportService.DeleteAsync(id);
            if (!response.Flag)
                return BadRequest(response.Message);

            return NoContent();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAsync(CreateAirportDto request,int id)
        {
            if(!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var response = await _airportService.UpdateAsync(request,id);
            if(!response.Flag)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var airports = await _airportService.GetAllAsync();

            if (airports == null)
                return NotFound();

            return Ok(airports);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var airport = await _airportService.GetByIdAsync(id);   

            if(airport == null) return NotFound();

            return Ok(airport);
        }
    }
}
