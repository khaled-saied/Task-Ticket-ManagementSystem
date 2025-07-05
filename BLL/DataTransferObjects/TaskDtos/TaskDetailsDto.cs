using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DataTransferObjects.ProjectDtos;
using BLL.DataTransferObjects.TicketDtos;

namespace BLL.DataTransferObjects
{
    public class TaskDetailsDto
    {
        //Id, Title, Description, Status, DueDate, ProjectDto, List<TicketDto>
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public ProjectDto Project { get; set; }
        public List<TicketDto> Tickets { get; set; }
        public string CtreatedBy { get; set; } = string.Empty; // User who created the task
        public string? AssignedTo { get; set; } // User to whom the task is assigned, nullable if not assigned
        public string? UserId { get; set; } // FK to ApplicationUser, nullable if not assigned
        public bool IsDueSoon { get; set; }  // for if the task is due soon
        public bool IsOverdue { get; set; }  // for if the task is overdue
    }
}
