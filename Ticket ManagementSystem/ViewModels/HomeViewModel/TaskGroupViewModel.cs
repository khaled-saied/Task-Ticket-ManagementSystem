using BLL.DataTransferObjects.CommentDtos;
using BLL.DataTransferObjects.TaskDtos;

namespace Ticket_ManagementSystem.ViewModels.HomeViewModel
{
    public class TaskGroupViewModel
    {
        public TaskDto Task { get; set; }
        public List<TicketGroupViewModel> Tickets { get; set; }
    }

}
