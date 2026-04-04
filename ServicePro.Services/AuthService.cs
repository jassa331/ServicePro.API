using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServicePro.Core.DTOs;
using ServicePro.Core.Entities;
using ServicePro.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace ServicePro.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository repository;
        private readonly IConfiguration config;
        private readonly HttpClient _httpClient;


        public AuthService(IAuthRepository repository, IConfiguration config)
        {
            this.repository = repository;
            this.config = config;
            _httpClient = new HttpClient();

        }
        private async Task<bool> VerifyCaptcha(string captchaResponse)
        {
            var secretKey = config["GoogleReCaptcha:SecretKey"];

            var response = await _httpClient.PostAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}",
                null);

            var jsonString = await response.Content.ReadAsStringAsync();

            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);

            return result.success == true;
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
            // 1️⃣ Verify Captcha First
            var isCaptchaValid = await VerifyCaptcha(dto.Captcha);

            if (!isCaptchaValid)
                throw new UnauthorizedAccessException("Captcha validation failed");

            // 2️⃣ User Check
            var user = await repository.GetUserByEmailAsync(dto.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            // 3️⃣ Generate JWT
            return GenerateJwt(user);
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
