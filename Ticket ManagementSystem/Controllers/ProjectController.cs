using System.Threading.Tasks;
using BLL.DataTransferObjects.ProjectDtos;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_ManagementSystem.Controllers
{
    //[Authorize]
    public class ProjectController(IServiceManger _serviceManger,
                                   ILogger<ProjectController> _logger,
                                   IWebHostEnvironment _environment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var Projects = await _serviceManger.ProjectService.GetAllProjects();
            return View(Projects);
        }

    }
}