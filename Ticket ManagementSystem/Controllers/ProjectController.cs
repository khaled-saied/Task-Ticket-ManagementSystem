using System.Threading.Tasks;
using BLL.DataTransferObjects.ProjectDtos;
using BLL.Exceptions;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_ManagementSystem.Controllers
{
    [Authorize]
    public class ProjectController(IServiceManger _serviceManger,
                                   ILogger<ProjectController> _logger,
                                   IWebHostEnvironment _environment,
                                   UserManager<ApplicationUser> _userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var Projects = await _serviceManger.ProjectService.GetAllProjects();
            return View(Projects);
        }

        #region Create 
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectDto createProjectDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var UserLogin = await _userManager.GetUserAsync(User)
                        ?? throw new NotFoundException("User not found");

                    var Result = await _serviceManger.ProjectService.CreateProject(createProjectDto,UserLogin);
                    if (Result > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Project creation failed");
                    }
                }
            }
            catch (Exception ex)
            {
                if (_environment.IsDevelopment())
                {
                    _logger.LogError(ex, "Error creating project");
                }
                else
                {
                    _logger.LogError("Error creating project");
                }
            }
            return View(createProjectDto);
        }

        #endregion

    }
}