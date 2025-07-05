using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DataTransferObjects.TaskDtos
{
    public class UpdateTaskDto
    {
        //Id, Title, Description, Status, DueDate
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string? UserId { get; set; } // FK to ApplicationUser, nullable if not assigned

    }
}
