using IntrumWebApi.Models;
using IntrumWebApi.Models.Entities;
using ItrumWebApi.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ItrumWebApi.Models
{
    public class RegistrationResponse : IIdentityResponse
    {
        public UserData UserData { get; set; }
        public IEnumerable<IdentityError>? Errors { get; private set; } = null!;

        public RegistrationResponse(IdentityUser user)
        {
            UserData = new UserData(user.Id, user.UserName);

            Errors = null;
        }
    }
}