using BaseEntity.Enums;
using BaseEntity.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<CurrencyResponse> ConvertAmount(decimal amount, CurrencyCode to);
    }
}
