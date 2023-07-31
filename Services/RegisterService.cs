using IntrumWebApi.Models.Entities;
using IntrumWebApi.Services;
using ItrumWebApi.Models;
using Microsoft.AspNetCore.Identity;

namespace PaymentApi.Services
{

    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest model)
        {
            ApplicationUser user = new ApplicationUser { UserName = model.UserName };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return new RegistrationResponse(user, result.Errors);

            return new RegistrationResponse(user);
        }
    }
}