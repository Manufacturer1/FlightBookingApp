using Stripe;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentIntent> CreateCharge(decimal amount, string currency = "usd");
    }
}
