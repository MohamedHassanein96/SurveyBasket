using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SurveyBasket.Settings;

namespace SurveyBasket.Services
{
    public class EmailSenderService(IOptions<MailSettings> mailSettings , ILogger<EmailSenderService> logger) :IEmailSender
    {
        private readonly MailSettings _mailSettings = mailSettings.Value;
        private readonly ILogger<EmailSenderService> _logger = logger;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail),
                Subject = subject
            };
            message.To.Add(MailboxAddress.Parse(email));

            var builder = new BodyBuilder
            {
                HtmlBody =htmlMessage
            };

            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            _logger.LogInformation("Sending email to email {email}", email);
            smtp.Connect(_mailSettings.Host, _mailSettings.Port ,SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(message);
            smtp.Disconnect(true);
        }
    }
}
