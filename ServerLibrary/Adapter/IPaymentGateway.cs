using BaseEntity.PaymentDtos;
using BaseEntity.Responses;

namespace ServerLibrary.Adapter
{
    public interface IPaymentGateway
    {
        Task<PaymentResponse> ProcessPayment(CreatePaymentDto createPayment);
    }
}
