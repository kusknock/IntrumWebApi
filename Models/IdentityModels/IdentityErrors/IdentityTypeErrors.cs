using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItrumWebApi.Models.IdentityModels
{
    public static class IdentityTypeErrors
    {
        public static readonly string UserNotFound = "User is not found";
        public static readonly string InvalidUserNameOrPassword = "You entered invalid password or username. Please try again";
        public static readonly string TokenIsInvalidOrNotFound = "Refresh token is invalid or not found in db";
    }
}
