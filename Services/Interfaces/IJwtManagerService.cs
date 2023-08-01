using IntrumWebApi.Models;
using IntrumWebApi.Models.Entities;
using ItrumWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace IntrumWebApi.Services
{
    public interface IJwtManagerRepository
    {
        TokenModel CreatePairOfTokens(IEnumerable<Claim> claims);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    }
}