using IntrumWebApi.Models.Entities;
using IntrumWebApi.Models.IdentityModels;
using ItrumWebApi.Models;
using ItrumWebApi.Models.IdentityModels;

namespace IntrumWebApi.Services
{
    public interface IUserService
    {
        Task<IIdentityResponse> Register(RegistrationRequest model);
        Task<IIdentityResponse> RegisterAdmin(RegistrationRequest model);
        Task<IIdentityResponse> Authenticate(AuthenticateRequest model);
        Task<IIdentityResponse> Revoke(string userName);
        Task RevokeAll();
        IEnumerable<ApplicationUser> GetAll();
        Task<ApplicationUser> GetById(string id);
    }
}