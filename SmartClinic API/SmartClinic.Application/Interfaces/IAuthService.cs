using SmartClinic.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(
            LoginRequestDto request);
    }
}
