using Ticket_ManagementSystem.Helper;
using Twilio.Rest.Api.V2010.Account;

namespace Ticket_ManagementSystem.Utilities
{
    public interface ISmsService
    {
        Task<MessageResource> SendSms(SmsMessage smsMessage);
    }
}
