using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IntrumWebApi.Models;
using System.Threading.Tasks;
using ItrumWebApi.Models;
using IntrumWebApi.Filters;
using System.IdentityModel.Tokens.Jwt;
using IntrumWebApi.Services.Account.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace IntrumWebApi.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private IUserService userService;
        private IConfiguration configuration;

        public AccountController(IUserService userService, IConfiguration configuration)
        {
            this.userService = userService;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistrationRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await userService.RegisterAsync(model);

            if (response.Errors != null)
                return BadRequest(response.Errors);

            return Ok(response);
        }

        [HttpPost("register-admin")]
        [LocalOnly]
        public async Task<IActionResult> RegisterAdmin(RegistrationRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await userService.RegisterAdminAsync(model);

            if (response.Errors != null)
                return BadRequest(response.Errors);

            return Ok(response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AuthenticateRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await userService.LoginAsync(model) as AuthenticateResponse;

            if (response?.Errors != null)
                return BadRequest(response?.Errors);

            _ = int.TryParse(configuration["Jwt:RefreshTokenExpiresIn"], out int RefreshTokenExpiresIn);

            Response.Cookies.Append("X-Refresh-Token", response.Tokens.RefreshToken,
                new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.Now.AddMinutes(RefreshTokenExpiresIn)
                });

            return Ok(response);
        }

        [HttpGet("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            Request.Cookies.TryGetValue("X-Refresh-Token", out string? refreshToken);

            Response.Cookies.Delete("X-Refresh-Token");

            var response = await userService.LogoutAsync(refreshToken ?? string.Empty);

            if (response?.Errors != null)
                return Unauthorized(response?.Errors);

            return Ok(response);
        }

        [HttpGet("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                Request.Cookies.TryGetValue("X-Refresh-Token", out string? refreshToken);

                Response.Cookies.Delete("X-Refresh-Token");

                var response = await userService.RefreshTokenAsync(refreshToken ?? string.Empty) as AuthenticateResponse;

                if (response is null || response.Errors != null)
                    return Unauthorized(response?.Errors);

                _ = int.TryParse(configuration["Jwt:RefreshTokenExpiresIn"], out int RefreshTokenExpiresIn);

                Response.Cookies.Append("X-Refresh-Token", response.Tokens.RefreshToken,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.Now.AddMinutes(RefreshTokenExpiresIn)
                    });

                return Ok(response);
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
