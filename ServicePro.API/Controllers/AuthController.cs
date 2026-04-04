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
        [HttpPost]
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
            try
            {
                var token = await authService.LoginAsync(dto);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }

    }
}