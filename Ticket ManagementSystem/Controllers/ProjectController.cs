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
                return View(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project details");
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

    }
}