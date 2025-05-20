using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DataTransferObjects.CommentDtos
{
    public class CreateCommentDto
    {
        //Content, TicketId
        public string Content { get; set; } = string.Empty;
        public int TicketId { get; set; }
    }
}
