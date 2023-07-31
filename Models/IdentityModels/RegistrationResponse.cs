using IntrumWebApi.Models.Entities;
using ItrumWebApi.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ItrumWebApi.Models
{
    public class RegistrationResponse : IIdentityResponse
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public IEnumerable<IdentityError>? Errors { get; private set; } = null!;

        public RegistrationResponse(ApplicationUser user)
        {
            Id = user.Id;
            Username = user.UserName;
            Errors = null;
        }

        public RegistrationResponse(ApplicationUser user, IEnumerable<IdentityError> errors)
        {
            Id = "-1";
            Username = user.UserName;
            Errors = errors;
        }
    }
}