using BaseEntity.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Observer;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotificationAsync(SendNotificationDto request)
        {
            if (request == null)
                return BadRequest("Request is null");

            var response = await _notificationService.SendBookingConfirmation(request);
            if(!response.Flag)
                return BadRequest(response.Message);

            return Ok(response);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAsync([FromBody] MarkNotificationReadDto request,int id)
        {
            if (id <= 0) return BadRequest("Id cannot less or equal to zero.");

            var response = await _notificationService.UpdateAsync(request, id);
            if (!response.Flag) return BadRequest(response.Message);

            return Ok(response.Message);
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            var notifications = await _notificationService.GetAllInAppNotifications();

            return Ok(notifications);
        }
        [HttpGet("get-notifications-by-email/{email}")]
        public async Task<IActionResult> GetNotificationsByEmail(string email)
        {
            var notifications = await _notificationService.GetNotificationsByPassengerEmailAsync(email);
            if (notifications == null)
                return NotFound("Notification were not found or there is not passenger with given email");

            return Ok(notifications);
        }
    }
}
