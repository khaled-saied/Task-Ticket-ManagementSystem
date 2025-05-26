using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DataTransferObjects.ProjectDtos;
using BLL.DataTransferObjects.TicketDtos;

namespace BLL.DataTransferObjects
{
    public class TaskDetailsDto
    {
        //Id, Title, Description, Status, DueDate, ProjectDto, List<TicketDto>
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public ProjectDto Project { get; set; }
        public List<TicketDto> Tickets { get; set; }
    }
}
