using IntrumWebApi.Models;
using IntrumWebApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IntrumWebApi.Services.Account.Interfaces
{
    public interface IRoleService
    {
        public Task<IEnumerable<Claim>> GetClaimsByUser(IdentityUser user);
        public Task SetRoleByUser(IdentityUser user, params UserRoles[] roles);
    }
}
