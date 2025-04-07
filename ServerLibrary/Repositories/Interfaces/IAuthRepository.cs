using BaseEntity.Entities;
using Microsoft.AspNetCore.Identity;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterAsync(ApplicationUser user, string password);
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task CreateRoleAsync(string role);
        Task AssignRoleAsync(ApplicationUser user, string role);
        Task<bool> RoleExistsAsync(string role);
        Task<IEnumerable<string>> GetUserRole(string userId);
    }
}
