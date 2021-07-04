using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Users;
using MediService.ASP.NET_Core.Services;
using System.Threading.Tasks;
using System.Linq;

namespace MediService.ASP.NET_Core.Controllers
{
    public class UsersController : Controller
    {
        private readonly MediServiceDbContext data;
        private readonly IPasswordHasher passwordHasher;

        public UsersController(MediServiceDbContext data, IPasswordHasher passwordHasher)
        {
            this.data = data;
            this.passwordHasher = passwordHasher;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = new User()
            {
                Username = model.Username,
                Email = model.Address,
                Password = this.passwordHasher.HashPassword(model.Password),
                FullName = model.FullName,
                IsSpecialist = false,
                PhoneNumber = model.Phone,
            };
            user.Addresses.Add(new Address()
            {
                City = model.City,
                FullAddress = model.Address,
            });

            this.data.Users.Add(user);
            await this.data.SaveChangesAsync();

            return Redirect("/Users/Login");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(UserLoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = this.data.Users
                .FirstOrDefault(x => x.Username == model.Username);

            if (user == null)
            {
                return NotFound($"Invalid username or password.");
            }
            
            return Redirect("/Home");
        }
    }
}
