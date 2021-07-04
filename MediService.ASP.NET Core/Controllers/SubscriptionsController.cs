using Microsoft.AspNetCore.Mvc;

namespace MediService.ASP.NET_Core.Controllers
{
    public class SubscriptionsController : Controller
    {
        public IActionResult All() => View();
    }
}
