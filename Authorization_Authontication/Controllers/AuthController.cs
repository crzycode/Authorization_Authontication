using Authorization_Authontication.Models;
using Authorization_Authontication.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization_Authontication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterServices registerServices;
        private readonly IConfiguration configuration;
        public AuthController(IRegisterServices _registerServices, IConfiguration _configuration)
        {
            registerServices = _registerServices;
            configuration = _configuration;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await registerServices.RegisterUser(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("invalid Properties");
         }
        [HttpGet("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await registerServices.LoginUser(model);
                    if (result.IsSuccess)
                    {
                        return Ok(result);
                    }
                    return BadRequest(result);
                }
                return BadRequest("Invalid Properties");

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet("ConfirmEmail")] 
        public async Task<IActionResult> Confirmemail(string userid,string token)
        {
            if(string.IsNullOrWhiteSpace(userid) || string.IsNullOrWhiteSpace(token))
            {
                return NotFound();
            }
            var result = await registerServices.CofirmEmailAsync(userid, token);
            if (result.IsSuccess)
            {
                return Redirect($"{configuration["BaseUrl"]}/Confirmemail.html");
            }
            return BadRequest(result);
        }
    }
}
