using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DataTransferObjects.CommentDtos;
using BLL.DataTransferObjects.TaskDtos;

namespace BLL.DataTransferObjects.TicketDtos
{
    public class TicketDetailsDto
    {
        //Id, Title, Type, Status, TaskDto, List<CommentDto>
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public TaskDto Task { get; set; } 
        public List<CommentDto> Comments { get; set; } = [];

    }


}
