using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models.Enums;

namespace DAL.Models
{
    public class Ticket : BaseEntity<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketTypeEnum Type { get; set; } // Enum for ticket type    
        public TicketStatusEnum Status { get; set; } // Enum for ticket status

        public int TaskId { get; set; } // FK to Task

        //Navigation properties
        public TaskK Task { get; set; } // Navigation property to Task
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
