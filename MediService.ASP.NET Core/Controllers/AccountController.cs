using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Users;

namespace MediService.ASP.NET_Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager = null)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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

            var address = new Address()
            {
                City = model.City,
                FullAddress = model.Address,
            };
            var user = new User()
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.Phone,
            };
            user.Addresses.Add(address);
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if (error.Code == "DuplicateUserName")
                    {
                        ModelState.AddModelError(nameof(model.Username), error.Description);
                    }
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
                return View();
            }
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(model.Username), "Invalid username or password.");
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
