using BLL.DataTransferObjects.TaskDtos;
using DAL.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ticket_ManagementSystem.ViewModels.ViewModelOfTask
{
    public class TaskIndexViewModel
    {
        public IEnumerable<TaskDto> Tasks { get; set; } = new List<TaskDto>();

        public TaskStatusEnum? StatusFilter { get; set; }

        public SelectList StatusList { get; set; }
    }
}
