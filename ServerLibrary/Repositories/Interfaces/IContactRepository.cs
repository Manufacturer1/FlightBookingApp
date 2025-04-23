using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Task<GeneralReponse> CreateAsync(ContactDetails contactDetails);
        Task<GeneralReponse> RemoveAsync(int id);
        Task<ContactDetails?> GetAsync(int id);
        Task<IEnumerable<ContactDetails>> GetAllAsync();
    }
}
