using Microsoft.Extensions.Options;
using New_School_Management_API.EmailService.EmailModel;
using System.Net.Mail;
using System.Net;

namespace New_School_Management_API.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ApplicationSettings _appSettings;

        public EmailService(IOptions<EmailSettings> emailSettings, IOptions<ApplicationSettings> appSettings)
        {
            _emailSettings = emailSettings.Value;
            _appSettings = appSettings.Value;
        }
        public async Task SendLoginNotificationAsync(string recipientEmail, string ipAddress, DateTime loginTime)
        {
            var subject = _appSettings.LoginNotificationSubject;
            var body = $@"
            <h2>New login to your {_appSettings.ApplicationName} account</h2>
            <p>We detected a login to your account:</p>
            <ul>
                <li><strong>Time:</strong> {loginTime.ToString("f")}</li>
                <li><strong>IP Address:</strong> {ipAddress}</li>
            </ul>
            <p>If this wasn't you, please secure your account immediately.</p>
            <p>Thank you,<br/>{_appSettings.ApplicationName} Team</p>";

            await SendEmailAsync(recipientEmail, subject, body);
        }



        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
            {
                client.EnableSsl = _emailSettings.EnableSsl;
                client.UseDefaultCredentials = _emailSettings.UseDefaultCredentials;
                client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = _emailSettings.IsBodyHtml
                };

                mailMessage.To.Add(recipientEmail);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
    
 }

