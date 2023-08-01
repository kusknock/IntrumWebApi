using Microsoft.AspNetCore.Identity;
using ItrumWebApi.Models.IdentityModels;
using System.Collections.Generic;
using IntrumWebApi.Models.Entities;
using IntrumWebApi.Models;

namespace ItrumWebApi.Models
{
    public class AuthenticateResponse : IIdentityResponse
    {
        public string Id { get; set; }
        public string? Username { get; set; }
        public TokenModel? TokenModel { get; set; }

        public IEnumerable<IdentityError>? Errors { get; private set; } = null!;

        public AuthenticateResponse(ApplicationUser user, string? token)
        {
            Id = user.Id;
            Username = user.UserName;
            TokenModel = new (token, user.RefreshToken);
            Errors = null;
        }
    }
}