using System.Threading.Tasks;
using BLL.Services.Interfaces;
using DAL.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    }
}
