using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorization_Authontication.Services.Interfaces
{
   public interface ISecurityService
    {
       public void SecureTokens(Claim[] claims, out JwtSecurityToken token, out string tokenstring);
    }
}
