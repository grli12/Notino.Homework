using MimeKit;

namespace Homework.Services.Emails
{
    public interface IEmailService
    {
         Task SendEmail(List<string> to, string subject, string htmlBody, AttachmentCollection attachments);
    }
}
