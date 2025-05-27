using System.Threading.Tasks;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_ManagementSystem.Controllers
{
    public class TicketController(IServiceManger _serviceManger,
                                  ILogger<TicketController> _logger,
                                  IWebHostEnvironment _environment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var tickets = await _serviceManger.TicketService.GetAllAsync();
            return View(tickets);
        }
    }
}
