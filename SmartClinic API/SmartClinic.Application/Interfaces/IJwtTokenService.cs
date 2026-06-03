using SmartClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(
            User user,
            string roleName);
    }
}
