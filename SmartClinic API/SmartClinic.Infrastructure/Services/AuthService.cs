using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;
using SmartClinic.Infrastructure.Persistence;
namespace SmartClinic.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext
            _context;

        private readonly IJwtTokenService
            _jwtTokenService;

        public AuthService(
            ApplicationDbContext context,
            IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<LoginResponseDto>LoginAsync(LoginRequestDto request)
        {
            var user =await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x =>x.Username ==request.Username && x.IsDeleted == false);

            if (user == null)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    Message ="Invalid username or password"
                };
            }

            bool isPasswordValid =BCrypt.Net.BCrypt.Verify(request.Password,user.PasswordHash);

            if (!isPasswordValid)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    Message ="Invalid username or password"
                };
            }

            string roleName =user.Role?.Name ?? "User";

            var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.UserId == user.Id);

            string token =_jwtTokenService.GenerateToken(user,roleName);

            return new LoginResponseDto
            {
                IsSuccess = true,
                Token = token,
                UserId = user.Id,
                Username =
                    user.Username ?? "",
                Role = roleName,
                DoctorId=doctor != null ? doctor.Id : 0,
                Message ="Login successful"
            };
        }
    }
}
