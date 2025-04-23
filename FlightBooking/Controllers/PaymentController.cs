using BaseEntity.PaymentDtos;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Adapter;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentGateway _paymentGateway;

        public PaymentController(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentDto request)
        {
            var response = await _paymentGateway.ProcessPayment(request);

            if (!response.Success)
                return BadRequest(response.ErrorMessage);

            return Ok(response);
        }
    }
}
