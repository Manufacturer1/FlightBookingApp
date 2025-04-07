using BaseEntity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Services.Implementations;
using ServerLibrary.Services.Interfaces;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService; 
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.Flag)
            {

                if (result.Message.Contains("is already taken") ||
                    result.Message.Contains("duplicate"))
                {
                    return Conflict(new { message = "User with this email already exists." });
                }
                else
                {
                    return BadRequest(new { message = "Something went wrong." });
                }
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginModel);

            if (!result.Flag) return Unauthorized(new { message = result.Message });

            return Ok(result);
        }

        [HttpGet("getusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetAllUsers();
            return Ok(users);
        }
    }
}
