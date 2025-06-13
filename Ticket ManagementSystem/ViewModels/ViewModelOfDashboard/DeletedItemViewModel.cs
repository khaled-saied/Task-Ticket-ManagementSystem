using BLL.DataTransferObjects.CommentDtos;
using BLL.DataTransferObjects.ProjectDtos;
using BLL.DataTransferObjects.TaskDtos;
using BLL.DataTransferObjects.TicketDtos;
using DAL.Models;

namespace Ticket_ManagementSystem.ViewModels.ViewModelOfDashboard
{
    public class DeletedItemViewModel
    {
        public IEnumerable<ProjectDto> projectDtos { get; set; } = new List<ProjectDto>();

        public IEnumerable<TaskDto> taskDtos { get; set; } = new List<TaskDto>();
        public IEnumerable<TicketDto> ticketDtos { get; set; } = new List<TicketDto>();
        public IEnumerable<CommentDto> commentDtos { get; set; } = new List<CommentDto>();
    }
}
