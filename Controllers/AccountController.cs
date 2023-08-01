using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IntrumWebApi.Models;
using IntrumWebApi.Services;
using System.Threading.Tasks;
using ItrumWebApi.Models;

namespace IntrumWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private IUserService userService;
        private IRegisterService registerService;

        public AccountController(IUserService userService, IRegisterService registerService)
        {
            this.userService = userService;
            this.registerService = registerService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegistrationRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await registerService.Register(model);

            if (response.Errors != null)
                return BadRequest(response.Errors);

            return Ok(response);
        }

        [HttpPost("register-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin([FromForm] RegistrationRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await registerService.RegisterAdmin(model);

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
    }
}
