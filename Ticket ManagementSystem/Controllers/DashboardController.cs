using System.Threading.Tasks;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticket_ManagementSystem.ViewModels;

namespace Ticket_ManagementSystem.Controllers
{

    [Authorize(Roles = "Admin,SuperAdmin")]
    public class DashboardController(IServiceManger _serviceManger,
                                    UserManager<ApplicationUser> _userManager,
                                    RoleManager<IdentityRole> _roleManager) : Controller
    {
        public async Task<IActionResult> Index()
        {

            var ticketDtos = await _serviceManger.TicketService.GetAllAsync();
            var taskDtos = await _serviceManger.TaskService.GetAllTasks();

            var Data = new DashboardViewModel()
            {
                CountOfUser = _userManager.Users.Count(),

                CountOfRoles = _roleManager.Roles.Count(),

                //Project
                CountOfProject = _serviceManger.ProjectService.GetCount().CountNow,
                CountOfAllProject = _serviceManger.ProjectService.GetCount().TotalCount,

                //Task
                CountOfTask = _serviceManger.TaskService.GetCount().CountNow,
                CountOfAllTask = _serviceManger.TaskService.GetCount().TotalCount,

                //Ticket
                CountOfTicket = _serviceManger.TicketService.GetCount().CountNow,
                CountOfAllTicket = _serviceManger.TicketService.GetCount().TotalCount,

                //Comment
                CountOfComment = _serviceManger.CommentService.GetCount().CountNow,
                CountOfAllComment = _serviceManger.CommentService.GetCount().TotalCount,

                //Status Of Ticket
                TicketDtos = ticketDtos,
                //Status Of Task
                TaskDtos = taskDtos
            };
            return View(Data);
        }
    }
}
