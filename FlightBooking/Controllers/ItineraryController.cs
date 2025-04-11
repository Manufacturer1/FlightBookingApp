using BaseEntity.Dtos;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Services.Interfaces;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItineraryController : ControllerBase
    {
        private readonly IItineraryService _itineraryService;

        public ItineraryController(IItineraryService itineraryService)
        {
            _itineraryService = itineraryService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromBody] CreateItineraryDto request)
        {
            if(!ModelState.IsValid) 
                return ValidationProblem(ModelState);


            var response = await _itineraryService.AddAsync(request);
            
            if(!response.Flag)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _itineraryService.GetAllAsync();

            return Ok(response);
        }
    }
}
