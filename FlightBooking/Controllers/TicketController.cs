using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("get-ticket/{id}")]
        public async Task<IActionResult> GetTicketAsync(int id)
        {
            if (id <= 0)
                return BadRequest("The id is not valid.");

            try
            {
                var ticket = await _ticketService.GetTicketAsync(id);
                return Ok(ticket);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("get-ticket-by-payment/{paymentIntentId}")]
        public async Task<IActionResult> GetTicketByPaymentIntentIdAsync(string paymentIntentId)
        {
            if (string.IsNullOrEmpty(paymentIntentId))
                return BadRequest("PaymentIntentId is null.");

            var tickets = await _ticketService.GetTicketByPaymentIntentIdAsync(paymentIntentId);

            if (tickets == null)
                return NotFound("Tickets were not found.");
;

            return Ok(tickets);
        }
    }
}
