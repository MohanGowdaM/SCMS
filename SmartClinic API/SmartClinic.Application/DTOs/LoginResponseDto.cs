using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class LoginResponseDto
    {
        public bool IsSuccess { get; set; }

        public string Token { get; set; }
            = string.Empty;

        public string Username { get; set; }
            = string.Empty;

        public string Role { get; set; }
            = string.Empty;

        public int UserId { get; set; }

        public string Message { get; set; }
            = string.Empty;

        public int DoctorId { get; set; }
    }
}
