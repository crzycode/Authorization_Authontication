using Authorization_Authontication.Models;
using Authorization_Authontication.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authorization_Authontication.Services
{
    public class RegisterServices : IRegisterServices
    {
        private readonly UserManager<IdentityUser> _Usermanager;
        private readonly ISecurityService _securityservice;
        private readonly IConfiguration _configuration;

        private readonly IMailServices _Mail;

        public RegisterServices(UserManager<IdentityUser> userManager,ISecurityService securityService, IConfiguration configuration
            ,IMailServices mailServices)
        {
            _Usermanager = userManager;
            _securityservice = securityService;
            _configuration = configuration;
            _Mail = mailServices;
        }

        public async Task<UserManagerResponse> RegisterUser(RegistrationViewModel model)
        {
            try
            {
                if (model == null)
                {

                }
                if (model.Password != model.ConfirmPassword)
                {
                    return new UserManagerResponse
                    {
                        Message = "password not match",
                        IsSuccess = false
                    };
                }

                var identityUser = new IdentityUser
                {
                    Email = model.Email,
                    UserName = model.Email
                };
                var result = await _Usermanager.CreateAsync(identityUser, model.Password);
                if (result.Succeeded)
                {
                    var confirmmailToken = await _Usermanager.GenerateEmailConfirmationTokenAsync(identityUser);
                    var encodedemail = Encoding.UTF8.GetBytes(confirmmailToken);
                    var validemailtoken = WebEncoders.Base64UrlEncode(encodedemail);
                    string url = $"{_configuration["BaseUrl"]}api/Auth/ConfirmEmail?userid={identityUser.Id}&token={validemailtoken}";
                    await _Mail.Sendemailasync(identityUser.Email, $"<h1>{url}</h1>");
                    return new UserManagerResponse
                    {
                        Message = $"user created",
                        IsSuccess = true
                    };

                }
                return new UserManagerResponse
                {
                    Message = $"{result}  user not created",
                    IsSuccess = true
                };

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<UserManagerResponse> LoginUser(LoginViewModel login)
        {
            try
            {
                var user = await _Usermanager.FindByEmailAsync(login.Email);
                if(user == null)
                {
                    return new UserManagerResponse
                    {
                        Message = "no User Email",
                        IsSuccess = false,
                    };
                }
                var result = await _Usermanager.CheckPasswordAsync(user, login.Password);
                if (!result)
                {
                    return new UserManagerResponse()
                    {
                        Message = "Invalid password",
                        IsSuccess = false,
                    };
                }
                var claim = new[]
                {
                    new Claim("Email",login.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };
                _securityservice.SecureTokens(claim, out JwtSecurityToken token, out string tokenstring);
                return new UserManagerResponse
                {
                    Message = tokenstring,
                    IsSuccess = true,
                    ExpireDate = token.ValidTo
                };



            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserManagerResponse> CofirmEmailAsync(string userId, string token)
        {
            try
            {
                var user = await _Usermanager.FindByIdAsync(userId);
                if(user == null)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = $"Invalid User{userId}"
                    };
                }
                var dtoken = WebEncoders.Base64UrlDecode(token);
                string normaltoken = Encoding.UTF8.GetString(dtoken);

                var result = await _Usermanager.ConfirmEmailAsync(user, normaltoken);
                if (result.Succeeded)
                {
                    return new UserManagerResponse
                    {
                        Message = "Email Confirm",
                        IsSuccess = true
                    };
                }
                return new UserManagerResponse
                {
                    Message = "email not confirm",
                    IsSuccess = false,
                    Error = result.Errors.Select(e => e.Description)
                };

            }
            catch (Exception)
            {

                throw;
            } 
        }
    }
}
