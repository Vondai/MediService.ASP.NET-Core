using Microsoft.AspNetCore.Mvc;

namespace MediService.ASP.NET_Core.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult All() => View();
    }
}
