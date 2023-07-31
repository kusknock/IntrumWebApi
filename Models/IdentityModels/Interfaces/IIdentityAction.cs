using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItrumWebApi.Models.IdentityModels
{
    public interface IIdentityResponse
    {
        IEnumerable<IdentityError> Errors { get; }
    }
}
