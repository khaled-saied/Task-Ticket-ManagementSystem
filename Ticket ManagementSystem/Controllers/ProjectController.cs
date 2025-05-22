using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_ManagementSystem.Controllers
{
    public class ProjectController(IServiceManger _serviceManger) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var Projects = await _serviceManger.ProjectService.GetAllProjects();
            return View(Projects);
        }
    }
}