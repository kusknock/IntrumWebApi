using ItrumWebApi.Models;
using ItrumWebApi.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace IntrumWebApi.Models.IdentityModels
{
    internal class RegularResponse : IIdentityResponse
    {
        public object? Model { get; }
        public IEnumerable<IdentityError>? Errors { get; }

        private RegularResponse(object? model, IEnumerable<IdentityError>? errors)
        {
            Errors = errors;
            Model = model;
        }

        public static RegularResponse ErrorResponse(object? model, string code, string description)
        {
            List<IdentityError> errors = new List<IdentityError>
                {
                    new IdentityError()
                    {
                        Code = code,
                        Description = description
                    }
                };

            return new RegularResponse(model, errors);
        }

        public static RegularResponse ErrorResponse(object? model, IEnumerable<IdentityError> errors)
        {
            return new RegularResponse(model, errors);
        }

        public static RegularResponse SuccessResponse(string? message)
        {
            return new RegularResponse(message, null);
        }
    }
}