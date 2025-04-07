using BaseEntity.Dtos;
using BaseEntity.Models;
using BaseEntity.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface IAuthService
    {
        Task<GeneralReponse> RegisterAsync(RegisterModel model);
        Task<LoginResponse> LoginAsync(LoginModel model);
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto?> GetUserByEmailAsync(string email);
    }
}
