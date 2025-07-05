using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        //FullName
        public string FullName { get; set; } = string.Empty;

        public string? ImageName { get; set; } = "staticImage.jpeg";

        //Navigation properties
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<TaskK> Tasks { get; set; } = new List<TaskK>();

    }
}
