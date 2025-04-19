using BaseEntity.Dtos;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Services.Interfaces;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;
        private readonly IFileService _fileService;
        public AmenityController(IAmenityService amenityService, IFileService fileService)
        {
            _amenityService = amenityService;
            _fileService = fileService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromBody] CreateAmenityDto request)
        {
            if(!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var response = await _amenityService.AddAsync(request);

            if(!response.Flag)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }
        [HttpPost("add-amenity-flights/{amenityId}")]
        public async Task<IActionResult> AddFlightsToAmenity(IEnumerable<FlightAmenityDto> request,int amenityId)
        {
            if (request == null)
                return BadRequest("Flight amenities cannot be null");

            var response = await _amenityService.AddFlightsToAmenities(request,amenityId);
            if (!response.Flag)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpPost("upload-icon")]
        public async Task<IActionResult> UploadAmenityIcon([FromForm] AmenityImageUploaderDto file)
        {
            if (file!.AmenityIcon == null || file!.AmenityIcon.Length == 0)
                return BadRequest("File is required.");

            if (file.AmenityIcon.Length > 1 * 1024 * 1024)
                return BadRequest("File size should not exceed 1 MB.");

            var allowedFileExtensions = new[] { ".jpg", ".jpeg", ".png", ".svg" };

            var imageName = await _fileService.SaveFileAsync(file!.AmenityIcon, allowedFileExtensions);

            return Ok(new { iconUrl = imageName });
        }

        [HttpPut("update-amenity/{amenityId}")]
        public async Task<IActionResult> UpdateAmenityAsync(UpdateAmenityDto request,int amenityId)
        {
            var response = await _amenityService.UpdateAmenityAsync(request,amenityId);

            if (!response.Flag) return BadRequest(response.Message);

            return Ok(response.Message);
        }
        [HttpPut("update-flight-amenities/{amenityId}")]
        public async Task<IActionResult> UpdateFlightAmenitiesAsync(IEnumerable<FlightAmenityDto> request,int amenityId)
        {
            var response = await _amenityService.UpdateFlightAmenitiesAsync(request, amenityId);

            if (!response.Flag) return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _amenityService.DeleteAsync(id);

            if(!result.Flag)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var amenities = await _amenityService.GetAllAsync();

            if(amenities == null)
                return NotFound();

            return Ok(amenities);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var amenity = await _amenityService.GetByIdAsync(id);

            if (amenity == null)
                return NotFound();

            return Ok(amenity);
        }

    }
}
