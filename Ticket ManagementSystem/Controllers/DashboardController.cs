using Microsoft.AspNetCore.Mvc;

namespace Ticket_ManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
