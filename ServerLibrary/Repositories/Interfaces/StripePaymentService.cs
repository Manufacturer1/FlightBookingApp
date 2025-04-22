using Stripe;

namespace ServerLibrary.Repositories.Interfaces
{
    public class StripePaymentService : IPaymentService
    {
        public async Task<PaymentIntent> CreateCharge(decimal amount, string currency = "usd")
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = currency,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },

            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            return intent;
        }
    }
}
