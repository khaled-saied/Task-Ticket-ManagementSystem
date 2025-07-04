using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DataTransferObjects.TaskDtos
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? AssignedToUserId { get; set; }  // Nullable to allow for unassigned tasks
        public DateTime DueDate { get; set; }
        public bool IsDueSoon { get; set; }  // for if the task is due soon
        public bool IsOverdue { get; set; }  // for if the task is overdue
    }
}
