using System.Threading.Tasks;
namespace QAP4.Domain.Extensions.Mail
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
