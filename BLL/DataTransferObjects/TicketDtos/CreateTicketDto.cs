using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DataTransferObjects.TicketDtos
{
    public class CreateTicketDto
    {
        //Title, Type, Description, TaskId ,status
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TaskId { get; set; } // FK to Task
        public string Status { get; set; } = string.Empty;

    }
}
