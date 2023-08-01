using IntrumWebApi.Models.Entities;
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

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);

            // проверяем существование пользователя
            if (user == null)
                return ErrorResponse(model, IdentityTypeErrors.UserNotFound, "User not found");

            var isPassValid = await userManager.CheckPasswordAsync(user, model.Password);

            // проверяем правильно ли он ввел пароль
            if (!isPassValid)
                return ErrorResponse(model, IdentityTypeErrors.InvalidUserNameOrPassword, "Invalid UserName or Password");

            var claims = await roleService.GetClaimsByUser(user);

            // генерируем токен записываем его в модель
            var jwtToken = jwtManager.CreateToken(claims.ToList());

            _ = double.TryParse(configuration["Jwt:RefreshTokenValidityInDays"], out double refreshTokenExpiredInDays);

            // создаем refresh token
            user.RefreshToken = jwtManager.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.Add(TimeSpan.FromDays(refreshTokenExpiredInDays));

            var resultUpdate = await userManager.UpdateAsync(user);

            // проверяем успешность обновления
            if (!resultUpdate.Succeeded)
                return new AuthenticateResponse(model, resultUpdate.Errors);

            return new AuthenticateResponse(user, new JwtSecurityTokenHandler().WriteToken(jwtToken));
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return userManager.Users.ToList();
        }

        public async Task<ApplicationUser> GetById(string id)
        {
            return await userManager.FindByIdAsync(id.ToString());
        }

        private AuthenticateResponse ErrorResponse(AuthenticateRequest model, string code, string description)
        {
            List<IdentityError> errors = new List<IdentityError>
                {
                    new IdentityError()
                    {
                        Code = code,
                        Description = description
                    }
                };

            return new AuthenticateResponse(model, errors);
        }
    }
}