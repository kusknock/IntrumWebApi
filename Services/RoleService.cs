using IntrumWebApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IntrumWebApi.Services.Interfaces
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityUser> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleService(RoleManager<IdentityUser> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<Claim>> GetClaimsByUser(ApplicationUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return authClaims;
        }
    }
}
