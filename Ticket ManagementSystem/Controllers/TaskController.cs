using System.Threading.Tasks;
using BLL.DataTransferObjects.TaskDtos;
using BLL.DataTransferObjects.TicketDtos;
using BLL.Services.Classes;
using BLL.Services.Interfaces;
using DAL.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Ticket_ManagementSystem.ViewModels;

namespace Ticket_ManagementSystem.Controllers
{
    public class TaskController(IServiceManger _serviceManger,
                                ILogger<TaskController> _logger,
                                IWebHostEnvironment _environment) : Controller
    {
        #region Index
        public async Task<IActionResult> Index(TaskStatusEnum? statusFilter)
        {
            // 1. Get all tasks 
            var tasks = await _serviceManger.TaskService.GetAllTasks();

            // 2. Apply filtering if user selected a status
            if (statusFilter.HasValue)
            {
                tasks = tasks
                    .Where(t => Enum.Parse<TaskStatusEnum>(t.Status) == statusFilter.Value)
                    .ToList();
            }

            // 3. Prepare ViewModel
            var viewModel = new TaskIndexViewModel
            {
                Tasks = tasks,
                StatusFilter = statusFilter,
                StatusList = new SelectList(Enum.GetValues(typeof(TaskStatusEnum)))
            };
            // 4. Send it to the View
            return View(viewModel);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create(int? projectId, string? returnUrl)
        {
            var projects = await _serviceManger.ProjectService.GetAllProjects();

            var viewModel = new CreateTaskViewModel
            {
                ProjectId = projectId ?? 0, // Set to 0 if no projectId is provided
                Projects = await GetProjectSelectListAsync(),
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var taskDto = new CreateTaskDto
                    {
                        Title = model.Title,
                        Description = model.Description,
                        DueDate = model.DueDate,
                        ProjectId = model.ProjectId
                    };
                    // 1. Call the service to create the task
                    await _serviceManger.TaskService.CreateTask(taskDto);
                    // 2. Redirect to the task index page
                    return RedirectToAction("Details", "Project", new { id = model.ProjectId });
                }
                catch (Exception ex)
                {
                    if (_environment.IsDevelopment())
                    {
                        _logger.LogError(ex, "Error creating task");
                    }
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the task. Please try again.");
                }
            }
            // If we got this far, something failed, redisplay the form
            model.Projects = await GetProjectSelectListAsync();
            return View(model);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int id)
        {
            if (id ==0 )
                return NotFound();
            var task = await _serviceManger.TaskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }
        #endregion


        #region Get Project
        private async Task<List<SelectListItem>> GetProjectSelectListAsync()
        {
            var projects = await _serviceManger.ProjectService.GetAllProjects();
            return projects.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
        }

        #endregion
    }
}
