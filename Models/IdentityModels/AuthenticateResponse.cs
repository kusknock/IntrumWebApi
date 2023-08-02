using Microsoft.AspNetCore.Identity;
using ItrumWebApi.Models.IdentityModels;
using System.Collections.Generic;
using IntrumWebApi.Models.Entities;
using IntrumWebApi.Models;

namespace ItrumWebApi.Models
{
    public class AuthenticateResponse : IIdentityResponse
    {
        public UserData UserData { get; set; }
        public TokenModel Tokens { get; set; }
        public IEnumerable<IdentityError>? Errors { get; private set; } = null!;

        public AuthenticateResponse(IdentityUser user, TokenModel tokens)
        {
            UserData = new UserData(user.Id, user.UserName);

            Tokens = new TokenModel(tokens.AccessToken, tokens.RefreshToken);

            Errors = null;
        }
    }
}