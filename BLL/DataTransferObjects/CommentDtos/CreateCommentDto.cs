using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DataTransferObjects.CommentDtos
{
    public class CreateCommentDto
    {
        //Content, TicketId
        [Required(ErrorMessage = "Comment content is required.")]
        [MaxLength(500, ErrorMessage = "Comment is too long.")]
        public string Content { get; set; } = string.Empty;
    }
}
