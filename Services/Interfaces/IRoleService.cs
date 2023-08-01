using IntrumWebApi.Models;
using IntrumWebApi.Models.Entities;
using System.Security.Claims;

namespace IntrumWebApi.Services.Interfaces
{
    public interface IRoleService
    {
        public Task<IEnumerable<Claim>> GetClaimsByUser(ApplicationUser user);
        public Task SetRoleByUser(ApplicationUser user, params UserRoles[] roles);
    }
}
