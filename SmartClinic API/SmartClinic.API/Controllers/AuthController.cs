using Microsoft.AspNetCore.Mvc;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;

namespace SmartClinic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService
            _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult>Login(LoginRequestDto request)
        {
            var result =await _authService.LoginAsync(request);

            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}
