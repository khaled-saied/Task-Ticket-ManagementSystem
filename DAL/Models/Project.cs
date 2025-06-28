using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Project : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;     
        public string UserId { get; set; } // FK to ApplicationUser

        //Navigation properties
        public virtual ICollection<TaskK> Tasks { get; set; } = new List<TaskK>();
        public virtual ApplicationUser User { get; set; } // Navigation property to ApplicationUser

    }
}
