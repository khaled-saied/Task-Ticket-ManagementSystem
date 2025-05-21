using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetAllAsync();
        Task<TicketDetailsDto> GetTicketById(int id);
        Task<int> CreateTicket(CreateTicketDto ticketDto);
        Task<int> UpdateTicket(UpdateTicketDto updateTicketDto);
        Task<bool> DeleteTicket(int id);
    }
}
