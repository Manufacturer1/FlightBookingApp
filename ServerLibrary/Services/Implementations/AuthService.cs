using BaseEntity.Configurations;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Models;
using BaseEntity.Responses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text;

namespace ServerLibrary.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository authRepository;
        private readonly IOptions<JwtSection> _config;
        public AuthService(IAuthRepository db, IOptions<JwtSection> config) { authRepository = db; _config = config; }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await authRepository.GetAllUsers();


            var usersDto = users.Select(x => new UserDto { UserId = x.Id, UserName = x.UserName, Email = x.Email, FullName = x.FullName }).ToList();

            foreach (var user in usersDto)
            {
                var roles = await authRepository.GetUserRole(user.UserId!);
                user.Roles = roles;
            }


            return usersDto;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await authRepository.FindByEmailAsync(email);

            if (user == null) return null;

            var userRoles = await authRepository.GetUserRole(user.Id);

            if (userRoles == null) return null;

            var userDto = new UserDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = userRoles,
                FullName = user.FullName,
            };

            return userDto;
        }

        public async Task<LoginResponse> LoginAsync(LoginModel model)
        {
            var user = await authRepository.FindByEmailAsync(model.Email!);
            if (user == null) return new LoginResponse(false, "Invalid email or password.");

            var isPasswordValid = await authRepository.CheckPasswordAsync(user, model.Password!);

            if (!isPasswordValid) return new LoginResponse(false, "Invalid email or password.");

            var roles = await authRepository.GetUserRole(user.Id);
            var token = GenerateJwtToken(user, roles);

            return new LoginResponse(true, "Login Successfully.", token);
        }

        public async Task<GeneralReponse> RegisterAsync(RegisterModel model)
        {

            var user = new ApplicationUser
            {
                UserName = model.Email,
                FullName = model.FullName,
                Email = model.Email,
            };
            var result = await authRepository.RegisterAsync(user, model.Password!);

            if (!result.Succeeded)
                return new GeneralReponse(false, string.Join(", ", result.Errors.Select(e => e.Description)));

            var roleExists = await authRepository.RoleExistsAsync(model.Role);
            if (!roleExists)
            {
                await authRepository.CreateRoleAsync(model.Role);
            }
            await authRepository.AssignRoleAsync(user, model.Role);

            return new GeneralReponse(true, "User registered successfuly.");
        }

        private string GenerateJwtToken(ApplicationUser user, IEnumerable<string> roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.FullName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("name", user.FullName),
                new Claim("roles", JsonSerializer.Serialize(roles))
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _config.Value.Issuer,
                audience: _config.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
