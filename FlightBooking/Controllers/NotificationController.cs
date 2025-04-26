using BaseEntity.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Services.Interfaces;

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
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            var notifications = await _notificationService.GetAllInAppNotifications();

            return Ok(notifications);
        }
    }
}
