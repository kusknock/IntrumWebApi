using IntrumWebApi.Models;
using IntrumWebApi.Models.Entities;
using ItrumWebApi.Models;
using ItrumWebApi.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace IntrumWebApi.Services.Account.Interfaces
{
    public interface ITokenService
    {
        Task<TokenModel> GenerateTokensAsync(IdentityUser user);
        ClaimsPrincipal ValidateAccessToken(string? accessToken);
        ClaimsPrincipal ValidateRefreshToken(string? refreshToken);
        Task<Token?> FindTokenAsync(string? refreshToken);
        Task<Token?> FindTokenByUserIdAsync(string? userId);
        Task<IIdentityResponse> RemoveTokenAsync(string refreshToken);
        Task SaveTokenAsync(string? userId, string? refreshToken);
    }
}