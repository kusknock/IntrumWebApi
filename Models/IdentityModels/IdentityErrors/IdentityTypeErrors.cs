using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItrumWebApi.Models.IdentityModels
{
    public static class IdentityTypeErrors
    {
        public static readonly string UserNotFound = "404";
        public static readonly string InvalidUserNameOrPassword = "400";
    }
}
