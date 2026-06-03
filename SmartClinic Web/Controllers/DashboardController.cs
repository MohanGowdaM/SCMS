using Microsoft.AspNetCore.Mvc;
using SmartClinic.Web.Filters;

namespace SmartClinic.Web.Controllers
{
    [RoleAuthorize("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
