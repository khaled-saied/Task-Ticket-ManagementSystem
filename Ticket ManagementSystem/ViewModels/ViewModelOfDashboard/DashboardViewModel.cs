using BLL.DataTransferObjects.TaskDtos;
using BLL.DataTransferObjects.TicketDtos;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace Ticket_ManagementSystem.ViewModels.ViewModelOfDashboard
{
    public class DashboardViewModel
    {

        public int CountOfUser { get; set; } 

        public int CountOfRoles { get; set; } 

        //Project
        public int CountOfProject { get; set; }
        public int CountOfAllProject { get; set; } 

        //Task
        public int CountOfTask { get; set; } 
        public int CountOfAllTask { get; set; }

        //Ticket
        public int CountOfTicket { get; set; } 
        public int CountOfAllTicket { get; set; } 

        //Comment
        public int CountOfComment { get; set; } 
        public int CountOfAllComment { get; set; }

        //Satus Of Ticket
        public IEnumerable<TicketDto> TicketDtos { get; set; } = new List<TicketDto>();

        // Status Of Task
        public IEnumerable<TaskDto> TaskDtos { get; set; } = new List<TaskDto>();

    }
}
