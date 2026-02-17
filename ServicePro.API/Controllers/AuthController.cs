using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicePro.Core.DTOs;
using ServicePro.Core.Interfaces;
using System.Security.Claims;

namespace ServicePro.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            //add comment test
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("please enter valid Candenstials ");
                }
                await authService.RegisterAsync(dto);
                return Ok("User registered successfully");
            }

            catch (Exception ex)
            {
                return BadRequest("something went wrong ");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await authService.LoginAsync(dto);
            return Ok(new { token });
        }
        [AllowAnonymous]
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var profile = await authService.GetProfileAsync(email);

            return Ok(profile);
        }

    }
}