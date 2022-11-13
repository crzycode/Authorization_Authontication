using Authorization_Authontication.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authorization_Authontication.Services
{
    public class SecureToken:ISecurityService
    {

        public SecureToken(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void SecureTokens(Claim[] claims, out JwtSecurityToken token, out string tokenstring)
        {
            try
            {

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:Key"]));
                token = new JwtSecurityToken(
                    issuer: Configuration["Authentication:Issuer"],
                    audience: Configuration["Authentication:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)

                    );
                tokenstring = new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {

                throw;
            }; ;
        }
    }
}
