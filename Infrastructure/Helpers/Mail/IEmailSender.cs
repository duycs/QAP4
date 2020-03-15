using System.Threading.Tasks;

namespace QAP4.Infrastructure.Helpers.Mail
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
