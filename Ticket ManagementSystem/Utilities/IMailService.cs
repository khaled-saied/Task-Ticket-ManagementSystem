using Ticket_ManagementSystem.Helper;

namespace Ticket_ManagementSystem.Utilities
{
    public interface IMailService
    {
        void Send(Email email);
    }
}
