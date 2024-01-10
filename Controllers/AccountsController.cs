using AccountService.Models;
using AccountService.Models.ViewModels;
using AccountService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        public AccountServices _accountServices;
        private readonly IConfiguration _config;
        public AccountsController(AccountServices services, IConfiguration config)
        {
            _accountServices = services;
            _config = config;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {

                var (result,loggedInUser) = await _accountServices.Login(model);   

                if (result.Succeeded)
                {
                    var token = GenerateToken(loggedInUser);
                    return Ok(token);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

                return Unauthorized(new { Error = new[] { "Invalid Login Attempt" } });

            }

            return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });


        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {

                var result = await _accountServices.Register(model);

                if (result.Succeeded)
                {

                    return Ok();
                }

                ModelState.AddModelError(string.Empty, "Invalid Registration");

                var erroMessage = result.Errors.Select(error => error.Description);

                return BadRequest(new { Errors = erroMessage });


            }
            return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
        }



        // To generate token
        private string GenerateToken(ApplicationUser user)
        {
            

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.DenyOnlyPrimarySid,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Email)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
