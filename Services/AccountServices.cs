using AccountService.Models;
using AccountService.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Services
{
    public class AccountServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public AccountServices(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }



        public async Task<Microsoft.AspNetCore.Identity.SignInResult> Login(LoginVM user)
        {

            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

            return result;
        }



        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> Register(RegisterVM model)
        {


            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            System.Console.Write("The error is : {0} ", result);

            if (result.Succeeded)
            {

                await _signInManager.SignInAsync(user, isPersistent: false);

            }

            return result;



        }
    }
}
