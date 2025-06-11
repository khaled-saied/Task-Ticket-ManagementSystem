using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Ticket_ManagementSystem.Helper;
using Ticket_ManagementSystem.Settings;

namespace Ticket_ManagementSystem.Utilities
{
    public class MailService(IOptions<MailSettings> _options) : IMailService
    {
        public void Send(Email email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Value.Email),
                Subject = email.Subject
            };

            mail.To.Add(MailboxAddress.Parse(email.To));

            mail.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));

            var builder = new BodyBuilder();

            builder.HtmlBody = email.Body;

            mail.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Connect(_options.Value.Host, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);

            smtp.Authenticate(_options.Value.Email, _options.Value.Password);

            smtp.Send(mail);

            smtp.Disconnect(true);
        }
    }
}
