using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IntrumWebApi.Services
{
    public class JwtManagerRepository : IJwtManagerRepository
    {
        private readonly IConfiguration configuration;

        public JwtManagerRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            _ = int.TryParse(configuration["Jwt:TokenValidityInMinutes"], out int tokenValidityInHours);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:ValidIssuer"],
                audience: configuration["Jwt:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInHours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
