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



        public async Task<(Microsoft.AspNetCore.Identity.SignInResult, ApplicationUser)> Login(LoginVM user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

            ApplicationUser loggedInUser = null;

            if (result.Succeeded)
            {
                loggedInUser = await _userManager.FindByEmailAsync(user.Email);
            }

            return (result, loggedInUser);
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
