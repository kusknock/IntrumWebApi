using IntrumWebApi.Models;
using IntrumWebApi.Models.Entities;
using IntrumWebApi.Models.IdentityModels;
using IntrumWebApi.Services.Interfaces;
using ItrumWebApi.Models;
using ItrumWebApi.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IntrumWebApi.Services.Account
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationContext context;
        private readonly IRoleService roleService;

        public TokenService(IConfiguration configuration, ApplicationContext context, IRoleService roleService)
        {
            this.configuration = configuration;
            this.context = context;
            this.roleService = roleService;
        }

        public async Task<TokenModel> GenerateTokensAsync(IdentityUser user)
        {
            var tokenModel = new TokenModel();

            var claims = await roleService.GetClaimsByUser(user);

            _ = int.TryParse(configuration["Jwt:AccessTokenExpiresIn"], out int AccessTokenExpiresIn);
            _ = int.TryParse(configuration["Jwt:RefreshTokenExpiresIn"], out int RefreshTokenExpiresIn);

            tokenModel.AccessToken = GenerateJwtToken(claims, configuration["JWT:SecretAccess"], AccessTokenExpiresIn);
            tokenModel.RefreshToken = GenerateJwtToken(claims, configuration["JWT:SecretRefresh"], RefreshTokenExpiresIn * 24 * 60);

            return tokenModel;
        }

        public async Task SaveTokenAsync(string? userId, string? refreshToken)
        {
            var tokenData = await FindTokenByUserIdAsync(userId);

            if (tokenData is null)
            {
                await context.Tokens.AddAsync(new Token
                {
                    UserId = userId,
                    RefreshToken = refreshToken
                });
            }
            else
            {
                tokenData.RefreshToken = refreshToken;
            }

            await context.SaveChangesAsync();
        }

        public async Task<Token?> FindTokenByUserIdAsync(string? userId)
        {
            return await context.Tokens
                            .Where(item => item.UserId == userId)
                            .SingleOrDefaultAsync();
        }

        public ClaimsPrincipal? ValidateAccessToken(string? accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretAccess"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public ClaimsPrincipal? ValidateRefreshToken(string? refreshToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretRefresh"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public async Task<Token?> FindTokenAsync(string? refreshToken)
        {
            return await context.Tokens
                .Where(item => item.RefreshToken == refreshToken)
                .SingleOrDefaultAsync();
        }

        public async Task<IIdentityResponse> RemoveTokenAsync(string refreshToken)
        {
            var tokenData = await FindTokenAsync(refreshToken);

            if (tokenData is null)
                return RegularResponse.ErrorResponse(refreshToken, nameof(IdentityTypeErrors.TokenIsInvalidOrNotFound),
                    IdentityTypeErrors.TokenIsInvalidOrNotFound);

            context.Tokens.Remove(tokenData);

            await context.SaveChangesAsync();

            return RegularResponse.SuccessResponse("Success!");
        }
        private string GenerateJwtToken(IEnumerable<Claim> authClaims, string secretString, int expiredInMinutes)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretString));

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(expiredInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
