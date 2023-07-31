using Microsoft.AspNetCore.Identity;
using ItrumWebApi.Models.IdentityModels;
using System.Collections.Generic;

namespace ItrumWebApi.Models
{
    public class AuthenticateResponse : IIdentityResponse
    {
        public string Id { get; set; }
        public string? Username { get; set; }
        public string? Token { get; set; } = null!;  

        public IEnumerable<IdentityError>? Errors { get; private set; } = null!;

        public AuthenticateResponse(IdentityUser user, string token)
        {
            Id = user.Id;
            Username = user.UserName;
            Token = token;
            Errors = null;
        }

        public AuthenticateResponse(AuthenticateRequest model, IEnumerable<IdentityError> errors)
        {
            Id = "-1";
            Username = model.UserName;
            Token = null;
            Errors = errors;
        }
    }
}