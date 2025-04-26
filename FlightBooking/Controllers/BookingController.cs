using BaseEntity.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Memento;
using ServerLibrary.Services.Interfaces;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("book-flight")]
        public async Task<IActionResult> BookFlightAsync(CreateBookingDto request)
        {
            if (request == null) return BadRequest("Request is null");

            var response = await _bookingService.BookSeatAsync(request);
            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response);
        }

        [HttpPost("save-state")]
        public IActionResult SaveState([FromBody] BookingDraft draft)
        {
            if (draft == null)
                return BadRequest("Draft is null");
            try
            {
                _bookingService.SaveBookingState(draft);
                return Ok("Booking state saved.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("current")]
        public IActionResult GetCurrentState()
        {
            var currentState = _bookingService.GetCurrentBookingState();

            if (currentState == null)
                return NotFound("There is not current state.");

            return Ok(currentState);
        }
        [HttpPost("clear-history")]
        public IActionResult ClearHistory()
        {
            _bookingService.ClearBookingHistory();

            return Ok("Booking history removed successfully");
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }
    }
}
