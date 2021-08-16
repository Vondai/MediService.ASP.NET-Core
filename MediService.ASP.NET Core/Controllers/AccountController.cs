using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Users;
using MediService.ASP.NET_Core.Services.Accounts;

namespace MediService.ASP.NET_Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IAccountService accounts;

        public AccountController
            (UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAccountService accounts)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.accounts = accounts;
        }
        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterFormModel model)
        {
            if (model.City != "Sofia")
            {
                ModelState.AddModelError(nameof(model.City), "Invalid city.");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = this.accounts.CreateUser(
                model.Username,
                model.Email,
                model.FullName,
                model.Phone, model.City,
                model.Address);
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            return Redirect("/Account/Login");
        }

        [AllowAnonymous]
        public IActionResult Login() => View();

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginFormModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return Redirect("/Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/Home");
        }

        public IActionResult AccessDenied()
        {
            return Unauthorized();
        }
    }
}
