using IntrumWebApi.Models.Entities;
using IntrumWebApi.Models.IdentityModels;
using ItrumWebApi.Models;
using ItrumWebApi.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace IntrumWebApi.Services.Account.Interfaces
{
    public interface IUserService
    {
        Task<IIdentityResponse> RegisterAsync(RegistrationRequest model);
        Task<IIdentityResponse> RegisterAdminAsync(RegistrationRequest model);
        Task<IIdentityResponse> LoginAsync(AuthenticateRequest model);
        Task<IIdentityResponse> LogoutAsync(string refreshToken);
        Task<IIdentityResponse> RefreshTokenAsync(string refreshToken);
        Task<IdentityUser> GetIdByUserNameAsync(string userName);
        Task<IdentityUser> GetByIdAsync(string id);
        IEnumerable<IdentityUser> GetAll();
    }
}