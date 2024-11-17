using Microsoft.AspNetCore.Identity;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Contracts
{
    public interface IAuthenticationService
    {

        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);

        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<string> CreateToken();


    }
}
