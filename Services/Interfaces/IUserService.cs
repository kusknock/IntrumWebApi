using IntrumWebApi.Models.Entities;
using ItrumWebApi.Models;

namespace IntrumWebApi.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        IEnumerable<ApplicationUser> GetAll();
        Task<ApplicationUser> GetById(string id);
    }
}