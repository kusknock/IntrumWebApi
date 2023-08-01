﻿using ItrumWebApi.Models;

namespace IntrumWebApi.Services
{
    public interface IRegisterService
    {
        Task<RegistrationResponse> Register(RegistrationRequest model);
        Task<RegistrationResponse> RegisterAdmin(RegistrationRequest model);
    }
}