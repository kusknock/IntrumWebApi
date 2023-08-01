using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IntrumWebApi.Models;
using IntrumWebApi.Services;
using System.Threading.Tasks;
using ItrumWebApi.Models;
using IntrumWebApi.Filters;

namespace IntrumWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegistrationRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await userService.Register(model);

            if (response.Errors != null)
                return BadRequest(response.Errors);

            return Ok(response);
        }

        [HttpPost("register-admin")]
        [LocalOnly]
        public async Task<IActionResult> RegisterAdmin([FromForm] RegistrationRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await userService.RegisterAdmin(model);

            if (response.Errors != null)
                return BadRequest(response.Errors);

            return Ok(response);
        }

        [HttpPost("auth")]
        [AllowAnonymous]
        public async Task<IActionResult> Auth([FromForm] AuthenticateRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await userService.Authenticate(model);

            if (response.Errors != null)
                return BadRequest(response.Errors);

            return Ok(response);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout(string userName)
        {
            var response = await userService.Revoke(userName);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            await userService.RevokeAll();

            return NoContent();
        }
    }
}
