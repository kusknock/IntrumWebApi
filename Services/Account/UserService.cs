using IntrumWebApi.Models;
using IntrumWebApi.Models.IdentityModels;
using IntrumWebApi.Services.Interfaces;
using ItrumWebApi.Models;
using ItrumWebApi.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace IntrumWebApi.Services.Account
{

    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IRoleService roleService;
        private readonly ITokenService tokenService;

        public UserService(UserManager<IdentityUser> userManager,
            IRoleService roleService,
            ITokenService tokenService)
        {
            this.userManager = userManager;
            this.roleService = roleService;
            this.tokenService = tokenService;
        }

        public async Task<IIdentityResponse> RegisterAsync(RegistrationRequest model)
        {
            IdentityUser user = new()
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

        public async Task<IIdentityResponse> RegisterAdminAsync(RegistrationRequest model)
        {
            IdentityUser user = new()
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

        public async Task<IIdentityResponse> LoginAsync(AuthenticateRequest model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);

            if (user == null)
                return RegularResponse.ErrorResponse(model, nameof(IdentityTypeErrors.UserNotFound), IdentityTypeErrors.UserNotFound);

            var isPassValid = await userManager.CheckPasswordAsync(user, model.Password);

            if (!isPassValid)
                return RegularResponse.ErrorResponse(model, nameof(IdentityTypeErrors.InvalidUserNameOrPassword), IdentityTypeErrors.InvalidUserNameOrPassword);

            TokenModel tokens = await tokenService.GenerateTokensAsync(user);

            await tokenService.SaveTokenAsync(user.Id, tokens.RefreshToken);

            return new AuthenticateResponse(user, tokens);
        }

        public IEnumerable<IdentityUser> GetAll()
        {
            return userManager.Users.ToList();
        }

        public async Task<IdentityUser> GetByIdAsync(string id)
        {
            return await userManager.FindByIdAsync(id.ToString());
        }

        public async Task<IdentityUser> GetIdByUserNameAsync(string userName)
        {
            return await userManager.FindByNameAsync(userName);
        }

        public async Task<IIdentityResponse> LogoutAsync(string refreshToken)
        {
            if (refreshToken is null)
                return RegularResponse.ErrorResponse(refreshToken, nameof(IdentityTypeErrors.TokenIsInvalidOrNotFound),
                    IdentityTypeErrors.TokenIsInvalidOrNotFound);

            var result = await tokenService.RemoveTokenAsync(refreshToken);

            return result;
        }

        public async Task<IIdentityResponse> RefreshTokenAsync(string? refreshToken)
        {
            if (refreshToken is null)
                return RegularResponse.ErrorResponse(refreshToken, nameof(IdentityTypeErrors.TokenIsInvalidOrNotFound),
                    IdentityTypeErrors.TokenIsInvalidOrNotFound);
            try
            {
                var userData = tokenService.ValidateRefreshToken(refreshToken);

                var tokenFromDb = await tokenService.FindTokenAsync(refreshToken);

                if (userData is null || tokenFromDb is null)
                    return RegularResponse.ErrorResponse(refreshToken, nameof(IdentityTypeErrors.TokenIsInvalidOrNotFound), IdentityTypeErrors.TokenIsInvalidOrNotFound);

                var user = await userManager.FindByNameAsync(userData?.Identity?.Name);

                TokenModel tokens = await tokenService.GenerateTokensAsync(user);

                await tokenService.SaveTokenAsync(user.Id, tokens.RefreshToken);

                return new AuthenticateResponse(user, tokens);
            }
            catch (SecurityTokenException ex)
            {
                return RegularResponse.ErrorResponse(refreshToken, nameof(IdentityTypeErrors.TokenIsInvalidOrNotFound), ex.Message);
            }
        }
    }
}