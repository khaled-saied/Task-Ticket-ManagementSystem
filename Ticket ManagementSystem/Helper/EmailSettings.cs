using System.Net;
using System.Net.Mail;

namespace Ticket_ManagementSystem.Helper
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            try
            {
                var Client = new SmtpClient("smtp.gmail.com", 587);
                Client.EnableSsl = true;
                //Client.Timeout = 10000; // Set timeout to 10 seconds
                Client.Credentials = new NetworkCredential("khaledsaied080@gmail.com", "kbzeipvkaomnheic");
                Client.Send("khaledsaied080@gmail.com", email.To,email.Subject, email.Body);

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                return false;
            }
        }
    }
}
