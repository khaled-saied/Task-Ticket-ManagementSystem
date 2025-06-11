

using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Ticket_ManagementSystem.Helper;
using Ticket_ManagementSystem.Settings;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Ticket_ManagementSystem.Utilities
{
    public class SmsService(IOptions<SmsSettings> _options) : ISmsService
    {
        public async Task<MessageResource> SendSms(SmsMessage smsMessage)
        {
            //1=> Conncect With Server

            TwilioClient.Init(_options.Value.AccountSID, _options.Value.AuthToken);

            //2=> Message
            var message = await MessageResource.CreateAsync(
                body: smsMessage.Body,
                from: new Twilio.Types.PhoneNumber(_options.Value.TwilioPhoneNumber),
                to: smsMessage.PhoneNumber
            );

            return message;
        }
    }
}
