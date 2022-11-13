using Authorization_Authontication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization_Authontication.Services.Interfaces
{
   public interface IRegisterServices
    {
        Task<UserManagerResponse> RegisterUser(RegistrationViewModel model);
        Task<UserManagerResponse> LoginUser(LoginViewModel Login);
        Task<UserManagerResponse> CofirmEmailAsync(string userId, string token);

    }
}
