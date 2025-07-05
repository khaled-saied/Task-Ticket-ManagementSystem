using BLL.DataTransferObjects.CommentDtos;
using BLL.DataTransferObjects.ProjectDtos;
using BLL.DataTransferObjects.TaskDtos;
using BLL.DataTransferObjects.TicketDtos;

namespace Ticket_ManagementSystem.ViewModels.HomeViewModel
{
    public class ProjectOverviewViewModel
    {
        public ProjectDto Project { get; set; }
        public List<TaskGroupViewModel> Tasks { get; set; }
    }
}
