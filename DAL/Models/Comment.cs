using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Comment : BaseEntity<int>
    {
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } 

        // Foreign Keys
        public int TicketId { get; set; }
        public string UserId { get; set; } = string.Empty;

        // Navigation Properties
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
