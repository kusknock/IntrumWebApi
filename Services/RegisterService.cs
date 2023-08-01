using IntrumWebApi.Models.Entities;
using IntrumWebApi.Services;
using IntrumWebApi.Services.Interfaces;
using ItrumWebApi.Models;
using Microsoft.AspNetCore.Identity;

namespace PaymentApi.Services
{

    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRoleService roleService;

        public RegisterService(UserManager<ApplicationUser> userManager, 
            IRoleService roleService)
        {
            this.userManager = userManager;
            this.roleService = roleService;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest model)
        {
            ApplicationUser user = new()
            {
                UserName = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return new RegistrationResponse(user, result.Errors);

            await roleService.SetRoleByUser(user, UserRoles.User);

            return new RegistrationResponse(user);
        }

        public async Task<RegistrationResponse> RegisterAdmin(RegistrationRequest model)
        {
            ApplicationUser user = new()
            {
                UserName = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return new RegistrationResponse(user, result.Errors);

            await roleService.SetRoleByUser(user, UserRoles.User, UserRoles.Admin);

            return new RegistrationResponse(user);
        }
    }
}