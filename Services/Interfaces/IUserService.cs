using ItrumWebApi.Models;
using Microsoft.AspNetCore.Identity;

namespace IntrumWebApi.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        IEnumerable<IdentityUser> GetAll();
        Task<IdentityUser> GetById(string id);
    }
}