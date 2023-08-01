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
        public TokenModel Tokens { get; set; }
        public IEnumerable<IdentityError>? Errors { get; private set; } = null!;

        public AuthenticateResponse(IdentityUser user, TokenModel tokens)
        {
            Id = user.Id;
            Username = user.UserName;
            Tokens = tokens;
            Errors = null;
        }
    }
}