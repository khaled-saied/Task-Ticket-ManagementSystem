using System.Threading.Tasks;
using BLL.DataTransferObjects.ProjectDtos;
using BLL.Exceptions;
using BLL.Services.Classes;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_ManagementSystem.Controllers
{
    //[Authorize(Roles ="Admin")]
    [Authorize]
    public class ProjectController(IServiceManger _serviceManger,
                                   ILogger<ProjectController> _logger,
                                   IWebHostEnvironment _environment,
                                   UserManager<ApplicationUser> _userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var Projects = await _serviceManger.ProjectService.GetAllProjects();
            if (User.IsInRole("Admin"))
                ViewBag.Layout = "~/Views/Shared/_DashboardLayout.cshtml";
            else
                ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
            return View(Projects);
        }

        #region Create 
        [HttpGet]
        public IActionResult Create()
        {
            if (User.IsInRole("Admin"))
                ViewBag.Layout = "~/Views/Shared/_DashboardLayout.cshtml";
            else
                ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";

            return View();
        }

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

        #region Update

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var project = await _serviceManger.ProjectService.GetProjectById(id);
                if (project == null)
                {
                    throw new NotFoundException("Project not found");
                }
                var updateDto = new UpdateProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description
                };

                if (User.IsInRole("Admin"))
                    ViewBag.Layout = "~/Views/Shared/_DashboardLayout.cshtml";
                else
                    ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";

                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project for update");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateProjectDto updateProjectDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var UserLogin = await _userManager.GetUserAsync(User)
                        ?? throw new NotFoundException("User not found");
                    var Result = await _serviceManger.ProjectService.UpdateProject( updateProjectDto );
                    if (Result > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Project update failed");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project");
            }
            return View(updateProjectDto);
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var project = await _serviceManger.ProjectService.GetProjectById(id);
                if (project == null)
                {
                    throw new NotFoundException("Project not found");
                }

                if (User.IsInRole("Admin"))
                    ViewBag.Layout = "~/Views/Shared/_DashboardLayout.cshtml";
                else
                    ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";

                return View(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project details");
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return BadRequest();
            try
            {
                var success = await _serviceManger.ProjectService.DeleteProject(id);
                if (!success)
                {
                    ModelState.AddModelError("", "Delete failed.");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_environment.IsDevelopment())
                {
                    //1- Devolpment=> log error in console and return error message to user
                    //ModelState.AddModelError("", ex.Message);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //2-Deployment=> log error in file or database and return  error view,
                    _logger.LogError(ex.Message);
                    return View("ErrorView", ex);
                }
            }
        }

        #endregion

    }
}