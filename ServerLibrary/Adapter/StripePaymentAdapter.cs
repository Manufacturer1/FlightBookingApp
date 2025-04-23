
using BaseEntity.PaymentDtos;
using BaseEntity.Responses;
using Microsoft.Extensions.Logging;
using ServerLibrary.Repositories.Interfaces;
using Stripe;

namespace ServerLibrary.Adapter
{
    public class StripePaymentAdapter : IPaymentGateway
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<StripePaymentAdapter> _logger;

        public StripePaymentAdapter(IPaymentService paymentService,ILogger<StripePaymentAdapter> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        public async Task<PaymentResponse> ProcessPayment(CreatePaymentDto createPayment)
        {
            try
            {
                var paymentIntent = await _paymentService.CreateCharge(createPayment.Amount, createPayment.Currency);
                var paymentResponse = new PaymentResponse
                {
                    ClientSecret = paymentIntent.ClientSecret,
                    Success = true,
                };
                return paymentResponse;
            }
            catch (StripeException ex)
            {
                _logger.LogDebug($"Stripe Error: {ex.StripeError?.Message ?? ex.Message}");
                return new PaymentResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message ?? "Something went wrong while proccessing the payment",
                };
            }
            catch(Exception ex)
            {
                _logger.LogDebug(ex.Message);
                return new PaymentResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message ?? "Something went wrong. An Exception Occured",
                };
            }
        }
    }
}
