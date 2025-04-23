using BaseEntity.Entities;
using BaseEntity.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IPassportIdentityRepository
    {
        Task<GeneralReponse> CreateAsync(PassportIdentity passportIdentity);
        Task<GeneralReponse> RemoveAsync(int id);
        Task<PassportIdentity?> GetByIdAsync(int id);
        Task<IEnumerable<PassportIdentity>> GetAllAsync();
    }
}
