using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace QAP4.Infrastructure.Helpers.Mail
{
    public class EmailSender : IEmailSender
    {
        private readonly string _sendGridKey;

        public EmailSender()
        {
            // get the configuration from the app settings
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            _sendGridKey = config["SendGridKey"];
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(_sendGridKey, subject, message, email);
        }

        private static Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                // should be a domain other than yahoo.com, outlook.com, hotmail.com, gmail.com
                From = new EmailAddress("mail@domain.com", subject),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            //var response = await client.SendEmailAsync(msg);
            return client.SendEmailAsync(msg);
        }
    }
}
