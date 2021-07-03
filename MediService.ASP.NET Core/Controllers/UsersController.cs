using Microsoft.AspNetCore.Mvc;

namespace MediService.ASP.NET_Core.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(int a)
        {
            return Redirect("/Users/Login");
        }
        public IActionResult Login() => View();
    }
}
