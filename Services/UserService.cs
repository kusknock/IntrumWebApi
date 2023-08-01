using IntrumWebApi.Models;
using IntrumWebApi.Models.Entities;
using IntrumWebApi.Models.IdentityModels;
using IntrumWebApi.Services;
using IntrumWebApi.Services.Interfaces;
using ItrumWebApi.Models;
using ItrumWebApi.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace PaymentApi.Services
{

    public class UserService : IUserService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRoleService roleService;
        private readonly IJwtManagerRepository jwtManager;

        public UserService(IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            IRoleService roleService,
            IJwtManagerRepository jwtManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.roleService = roleService;
            this.jwtManager = jwtManager;
        }

        public async Task<IIdentityResponse> Register(RegistrationRequest model)
        {
            ApplicationUser user = new()
            {
                UserName = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return RegularResponse.ErrorResponse(user, result.Errors);

            await roleService.SetRoleByUser(user, UserRoles.User);

            return new RegistrationResponse(user);
        }

        public async Task<IIdentityResponse> RegisterAdmin(RegistrationRequest model)
        {
            ApplicationUser user = new()
            {
                UserName = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return RegularResponse.ErrorResponse(user, result.Errors);

            await roleService.SetRoleByUser(user, UserRoles.User, UserRoles.Admin);

            return new RegistrationResponse(user);
        }

        public async Task<IIdentityResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);

            if (user == null)
                return RegularResponse.ErrorResponse(model, 
                    nameof(IdentityTypeErrors.UserNotFound), 
                    IdentityTypeErrors.UserNotFound);

            var isPassValid = await userManager.CheckPasswordAsync(user, model.Password);

            if (!isPassValid)
                return RegularResponse.ErrorResponse(model, 
                    nameof(IdentityTypeErrors.InvalidUserNameOrPassword), 
                    IdentityTypeErrors.InvalidUserNameOrPassword);

            var claims = await roleService.GetClaimsByUser(user);
            var tokens = jwtManager.CreatePairOfTokens(claims);

            _ = double.TryParse(configuration["Jwt:RefreshTokenValidityInDays"], out double refreshTokenExpiredInDays);
            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.Add(TimeSpan.FromDays(refreshTokenExpiredInDays));

            var resultUpdate = await userManager.UpdateAsync(user);

            // проверяем успешность обновления
            if (!resultUpdate.Succeeded)
                return RegularResponse.ErrorResponse(model, resultUpdate.Errors);

            return new AuthenticateResponse(user, tokens.AccessToken);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return userManager.Users.ToList();
        }

        public async Task<ApplicationUser> GetById(string id)
        {
            return await userManager.FindByIdAsync(id.ToString());
        }

        public async Task<IIdentityResponse> Revoke(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
                return RegularResponse.ErrorResponse(user, nameof(IdentityTypeErrors.UserNotFound), IdentityTypeErrors.UserNotFound);

            user.RefreshToken = null;

            await userManager.UpdateAsync(user);

            return RegularResponse.SuccessResponse("Success!");
        }

        public async Task RevokeAll()
        {
            var users = userManager.Users.ToList();

            foreach (var user in users)
            {
                user.RefreshToken = null;

                await userManager.UpdateAsync(user);
            }
        }


    }
}