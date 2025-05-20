using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DataTransferObjects.CommentDtos
{
    public class UpdateCommentDto
    {
        //Id, Content
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
