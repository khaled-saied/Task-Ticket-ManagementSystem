using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DataTransferObjects.TicketDtos;

namespace BLL.DataTransferObjects.CommentDtos
{
    public class CommentDetailsDto
    {
        //Id, Content, CreatedAt, Author(UserDto), TicketDto
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = string.Empty;
        public TicketDto? TicketDto { get; set; }
    }
}
