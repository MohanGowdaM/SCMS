using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartClinic.Application.Interfaces;
using SmartClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartClinic.Application.Services
{
    public class JwtTokenService
       : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(
            User user,
            string roleName)
        {
            var key = Encoding.UTF8.GetBytes(
                _configuration["JwtSettings:Key"]!);

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.Username ?? ""),

                new Claim(
                    ClaimTypes.Role,
                    roleName),

                new Claim(
                    "FullName",
                    user.Name ?? "")
            };

            var credentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer:
                        _configuration[
                            "JwtSettings:Issuer"],

                    audience:
                        _configuration[
                            "JwtSettings:Audience"],

                    claims: claims,

                    expires:
                        DateTime.UtcNow.AddMinutes(
                            Convert.ToDouble(
                                _configuration[
                                    "JwtSettings:DurationInMinutes"])),

                    signingCredentials:
                        credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
