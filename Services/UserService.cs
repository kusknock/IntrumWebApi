using IntrumWebApi.Configuration;
using IntrumWebApi.Services;
using ItrumWebApi.Models;
using ItrumWebApi.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PaymentApi.Services
{

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        private readonly UserManager<IdentityUser> _userManager;

        public UserService(IOptions<AppSettings> appSettings, UserManager<IdentityUser> userManager)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            // проверяем существование пользователя
            if (user == null)
                return ErrorResponse(model, IdentityTypeErrors.UserNotFound, "User not found");

            var isPassValid = await _userManager.CheckPasswordAsync(user, model.Password);

            // проверяем правильно ли он ввел пароль
            if (!isPassValid)
                return ErrorResponse(model, IdentityTypeErrors.InvalidUserNameOrPassword, "Invalid UserName or Password");

            // генерируем токен записываем его в модель
            var jwtToken = generateJwtToken(user);

            var resultUpdate = await _userManager.UpdateAsync(user);

            // проверяем успешность обновления
            if (!resultUpdate.Succeeded)
                return new AuthenticateResponse(model, resultUpdate.Errors);

            return new AuthenticateResponse(user, jwtToken);
        }

        public IEnumerable<IdentityUser> GetAll()
        {
            return _userManager.Users.ToList();
        }

        public async Task<IdentityUser> GetById(string id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
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