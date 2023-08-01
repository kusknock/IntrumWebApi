using IntrumWebApi.Models;
using IntrumWebApi.Services.Account.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IntrumWebApi.Services.Account
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public RoleService(RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<Claim>> GetClaimsByUser(IdentityUser user)
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

        public async Task SetRoleByUser(IdentityUser user, params UserRoles[] roles)
        {
            foreach (var role in roles)
            {
                var displayName = Enum.GetName(role);

                if (displayName == null)
                    continue;

                if (!await roleManager.RoleExistsAsync(displayName))
                    await roleManager.CreateAsync(new IdentityRole(displayName));

                await userManager.AddToRoleAsync(user, displayName);
            }
        }
    }
}
