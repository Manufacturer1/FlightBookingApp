using BaseEntity.Dtos;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Services.Interfaces;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;


        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateFlight([FromForm] CreateFlightDto createFlight)
        {
            var result = await _flightService.AddAsync(createFlight);
            if (!result.Flag)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("delete/{flightNumber}")]
        public async Task<IActionResult> DeleteFlight(string flightNumber)
        {
            var result = await _flightService.DeleteAsync(flightNumber);

            if (!result.Flag)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateFlight([FromForm] UpdateFlightDto request)
        {
            var result = await _flightService.UpdateAsync(request);

            if (!result.Flag)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpGet("flight/{flightNumber}")]
        public async Task<IActionResult> GetFlight(string flightNumber)
        {
            var result = await _flightService.GetByFlightNumberAsync(flightNumber);

            if (result == null)
                return NotFound("Flight was not found.");

            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFlights()
        {
            var result = await _flightService.GetAllAsync();

            return Ok(result);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchFlights([FromBody] FlightCardRequestDto request)
        {
            var result = await _flightService.GetFlightCards(request);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

    }
}
