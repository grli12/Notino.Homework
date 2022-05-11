using Homework.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Homework.Services.Emails
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettingsOptions)
        {
            this.smtpSettings = smtpSettingsOptions.Value;
        }

        public async Task SendEmail(List<string> addressees, string subject, string htmlBody, AttachmentCollection attachments)
        {
            if (addressees.Count == 0)
                return;

            MimeMessage email = BuildEmailMessage(addressees, subject, htmlBody, attachments);

            await Send(email);
        }

        private MimeMessage BuildEmailMessage(List<string> addressees, string subject, string htmlBody, AttachmentCollection attachments)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(this.smtpSettings.Address));
            message.Subject = subject;

            foreach (string addressee in addressees)
            {
                message.To.Add(MailboxAddress.Parse(addressee));
            }

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlBody;

            foreach (var attachment in attachments)
            {
                builder.Attachments.Add(attachment);
            }

            message.Body = builder.ToMessageBody();

            return message;
        }

        private async Task Send(MimeMessage email)
        {
            var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(this.smtpSettings.ServerHost, this.smtpSettings.ServerPort, SecureSocketOptions.SslOnConnect);
            await smtpClient.AuthenticateAsync(this.smtpSettings.User, this.smtpSettings.Password);
            await smtpClient.SendAsync(email);
            await smtpClient.DisconnectAsync(true);
        }
    }
}
