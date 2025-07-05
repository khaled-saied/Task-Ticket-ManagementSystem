using System.Threading.Tasks;
using BLL.DataTransferObjects.TaskDtos;
using BLL.DataTransferObjects.TicketDtos;
using BLL.Exceptions;
using BLL.Services.Classes;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Ticket_ManagementSystem.ViewModels;
using Ticket_ManagementSystem.ViewModels.ViewModelOfTask;

namespace Ticket_ManagementSystem.Controllers
{
    public class TaskController(IServiceManger _serviceManger,
                                ILogger<TaskController> _logger,
                                IWebHostEnvironment _environment,
                                UserManager<ApplicationUser> _userManager) : Controller
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
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> Create(int? projectId, string? returnUrl)
        {
            var viewModel = new CreateTaskViewModel
            {
                ProjectId = projectId ?? 0, // Set to 0 if no projectId is provided
                Projects = await GetProjectSelectListAsync(),
                Users = await GetUserSelectListAsync(),
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
                        ProjectId = model.ProjectId,
                        UserId = model.UserId
                    };
                    // 1. Call the service to create the task
                    var UserLogin = await _userManager.GetUserAsync(User)
                      ?? throw new NotFoundException("User not found");
                    await _serviceManger.TaskService.CreateTask(taskDto,UserLogin);
                    // 2. Redirect to the task index page
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);

                    return RedirectToAction("Index", "Task");
                }
                catch (Exception ex)
                {
                    if (_environment.IsDevelopment())
                    {
                        _logger.LogError(ex, "Error creating task");
                    }
                    ModelState.AddModelError(string.Empty, "This address may already exist and an error occurred while creating the task. Please try again!!.");
                }
            }
            // If we got this far, something failed, redisplay the form
            model.Projects = await GetProjectSelectListAsync();
            model.Users = await GetUserSelectListAsync();
            return View(model);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
                return NotFound();
            var task = await _serviceManger.TaskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            //return View(task);
            // Compute IsOverdue و IsDueSoon
            var now = DateTime.Now;
            var dueDate = task.DueDate;

            task.IsOverdue = (dueDate < now) && task.Status != "Completed";
            task.IsDueSoon = (dueDate >= now && dueDate <= now.AddDays(3)) && task.Status != "Completed";

            return View(task);

        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
                return NotFound();
            var task = await _serviceManger.TaskService.GetTaskById(id);
            if (task == null)
                return NotFound();

            if(User.Identity?.Name != task.CtreatedBy || User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
                return View("AccessDenied");


            var updateTaskViewModel = new UpdateTaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                UserId = task.UserId,
                Users = await GetUserSelectListAsync() // Populate the user select list
            };
            return View(updateTaskViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Call the service to update the task
                    var taskDto = new UpdateTaskDto
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Description = model.Description,
                        DueDate = model.DueDate,
                        Status = model.Status,
                        UserId = model.UserId // FK to ApplicationUser
                    };
                    await _serviceManger.TaskService.UpdateTask(taskDto);
                    // 2. Redirect to the task index page
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (_environment.IsDevelopment())
                    {
                        _logger.LogError(ex, "Error updating task");
                    }
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the task. Please try again.");
                }
            }
            return View(model);
        }

        #endregion

        #region Delete
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
                return NotFound();
            try
            {
                var task = await _serviceManger.TaskService.GetTaskById(id);
                if (task == null)
                {
                    TempData["ErrorMessage"] = "Task not found.";
                    return RedirectToAction("Index");
                }
                await _serviceManger.TaskService.DeleteTask(id);
                TempData["SuccessMessage"] = "Task deleted successfully.";

            }
            catch (Exception ex)
            {
                if (_environment.IsDevelopment())
                {
                    _logger.LogError(ex, "Error deleting task");
                }
                TempData["ErrorMessage"] = "An error occurred while deleting the task.";
            }
            return RedirectToAction("Index");
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

        #region Get User
        private async Task<List<SelectListItem>> GetUserSelectListAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.UserName
            }).ToList();
        }
        #endregion

    }
}
