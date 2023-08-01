using IntrumWebApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace IntrumWebApi.Services.Interfaces
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager)
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

        public async Task SetRoleByUser(ApplicationUser user, params UserRoles[] roles)
        {
            foreach(var role in roles)
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
