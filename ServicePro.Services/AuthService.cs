using Microsoft.IdentityModel.Tokens;
using ServicePro.Core.DTOs;
using ServicePro.Core.Entities;
using ServicePro.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ServicePro.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository repository;
        private readonly IConfiguration config;

        public AuthService(IAuthRepository repository, IConfiguration config)
        {
            this.repository = repository;
            this.config = config;
        }

        public async Task RegisterAsync(RegisterRequestDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = dto.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await repository.RegisterUserAsync(user);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            try { 
            var user = await repository.GetUserByEmailAsync(dto.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            return GenerateJwt(user);
        
            }

            catch (Exception ex)
            {
                return ("something went wrong ");
            }

        }
        private string GenerateJwt(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role) // 🔥 ROLE ADD HERE
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> GetProfileAsync(string email)
        {
            var user = await repository.GetUserByEmailAsync(email);

            if (user == null)
                throw new Exception("User not found");

            return user;
        }

    }
}
