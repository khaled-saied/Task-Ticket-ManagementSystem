using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DataTransferObjects.UserDtos
{
    public class UserDetailsDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<string>? Roles { get; set; }
        public List<CommentDto>? Comments { get; set; }
        public List<ProjectDto>? Projects { get; set; }
    }
}
