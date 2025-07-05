using BLL.DataTransferObjects.CommentDtos;
using BLL.DataTransferObjects.TicketDtos;

namespace Ticket_ManagementSystem.ViewModels.HomeViewModel
{
    public class TicketGroupViewModel
    {
        public TicketDto Ticket { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
