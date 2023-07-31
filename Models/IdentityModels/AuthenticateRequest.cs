﻿using Microsoft.AspNetCore.Identity;
using ItrumWebApi.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItrumWebApi.Models
{
    public class AuthenticateRequest 
    {
        [Required]
        public string? UserName { get; set; } 

        [Required]
        public string? Password { get; set; }
    }
}