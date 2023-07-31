using IntrumWebApi.Models.Entities;
using ItrumWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace IntrumWebApi.Services
{
    public interface IJwtManagerRepository
    {
        JwtSecurityToken CreateToken(List<Claim> authClaims);
        string GenerateRefreshToken();
    }
}