using BaseEntity.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Services.Interfaces;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaneController : ControllerBase
    {
        private readonly IPlaneService _planeService;
        public PlaneController(IPlaneService planeService)
        {
            _planeService = planeService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromBody] CreatePlaneDto request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            return Ok(await _planeService.AddAsync(request));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _planeService.DeleteAsync(id);

            if (!response.Flag)
                return BadRequest(response.Message);

            return NoContent();
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody] CreatePlaneDto request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var response = await _planeService.UpdateAsync(request,id);

            if(!response.Flag) return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var plane = await _planeService.GetByIdAsync(id);
            if (plane == null)
                return NotFound();

            return Ok(plane);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _planeService.GetAllAsync());
        }
    }
}
