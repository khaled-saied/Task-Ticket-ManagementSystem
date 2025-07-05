using System.Diagnostics;
using System.Threading.Tasks;
using BLL.DataTransferObjects.ProjectDtos;
using BLL.DataTransferObjects.TaskDtos;
using BLL.DataTransferObjects.TicketDtos;
using BLL.Services.Classes;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticket_ManagementSystem.Models;
using Ticket_ManagementSystem.ViewModels;
using Ticket_ManagementSystem.ViewModels.HomeViewModel;

namespace Ticket_ManagementSystem.Controllers;

[Authorize]
public class HomeController(IServiceManger _serviceManger, ILogger<HomeController> _logger) : Controller
{

    public async Task<IActionResult> Index()
    {
        var projects = await _serviceManger.ProjectService.GetAllProjects();
        var result = new List<ProjectOverviewViewModel>();

        foreach (var project in projects)
        {
            var projectDetails = await _serviceManger.ProjectService.GetProjectById(project.Id);
            var projectDto = new ProjectDto()
            {
                Id = projectDetails.Id,
                Name = projectDetails.Name,
                Description = projectDetails.Description
            };
            var taskList = new List<TaskGroupViewModel>();

            foreach (var task in projectDetails.Tasks)
            {
                var taskDetails = await _serviceManger.TaskService.GetTaskById(task.Id);
                var taskDto = new TaskDto()
                {
                    Id = taskDetails.Id,
                    Title = taskDetails.Title,
                    Status = taskDetails.Status,
                    DueDate = taskDetails.DueDate,
                    IsDueSoon = taskDetails.IsDueSoon,
                    IsOverdue = taskDetails.IsOverdue
                };
                var ticketList = new List<TicketGroupViewModel>();

                foreach (var ticket in taskDetails.Tickets)
                {
                    var ticketDetails = await _serviceManger.TicketService.GetTicketById(ticket.Id);
                    var ticketDto = new TicketDto()
                    {
                        Id = ticketDetails.Id,
                        Title = ticketDetails.Title,
                        Status = ticketDetails.Status
                    };

                    ticketList.Add(new TicketGroupViewModel
                    {
                        Ticket = ticketDto,
                        Comments = ticketDetails.Comments.ToList()
                    });
                }

                taskList.Add(new TaskGroupViewModel
                {
                    Task = taskDto,
                    Tickets = ticketList
                });
            }

            result.Add(new ProjectOverviewViewModel
            {
                Project = projectDto,
                Tasks = taskList
            });
        }

        return View(result); 
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
