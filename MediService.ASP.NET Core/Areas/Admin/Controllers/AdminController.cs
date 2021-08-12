using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static MediService.ASP.NET_Core.Areas.Admin.AdminConstants;

namespace MediService.ASP.NET_Core.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdminRoleName)]
    public abstract class AdminController : Controller
    {
    }
}
