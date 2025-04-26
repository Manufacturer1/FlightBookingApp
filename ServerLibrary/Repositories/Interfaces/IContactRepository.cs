using BaseEntity.Entities;
using BaseEntity.Responses;
using System.Linq.Expressions;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Task<int> CreateAsync(ContactDetails contactDetails);
        Task<GeneralReponse> UpdateAsync(ContactDetails contactDetails);
        Task<GeneralReponse> RemoveAsync(int id);
        Task<ContactDetails?> GetAsync(int id);
        Task<IEnumerable<ContactDetails>> GetAllAsync();
        Task<IEnumerable<ContactDetails>> FindAsync(Expression<Func<ContactDetails, bool>> predicate);
    }
}
