using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models.Enums;

namespace DAL.Models
{
    public class TaskK : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatusEnum Status { get; set; }
        public DateTime DueDate { get; set; } // ✅ New property for due date

        public int ProjectId { get; set; } // FK to Project
        public ICollection<Ticket> Tickets { get; set; } // Navigation property to Ticket

        public Project Project { get; set; } // Navigation property to Project

    }
}
