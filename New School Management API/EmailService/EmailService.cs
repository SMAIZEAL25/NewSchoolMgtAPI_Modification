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
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, IOptions<ApplicationSettings> appSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        // SendNotification of login to student once they complete the login 
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

        // SendNotification of registration to student once they complete the registration 
        public async Task SendRegistrationSuccessEmailAsync(string recipientEmail, string username)
        {
            try
            {
                var subject = $"Welcome to {_appSettings.ApplicationName}!";
                var loginUrl = $"{_appSettings.BaseUrl}/login";
                var supportEmail = _appSettings.SupportEmail;

                var body = $@"
            <h2>Welcome, {username}!</h2>
            <p>Your account with {_appSettings.ApplicationName} has been successfully created.</p>
            <p><a href='{loginUrl}'>Click here to login</a> and get started.</p>
            
            <h3>Getting Started</h3>
            <ul>
                <li>Complete your profile</li>
                <li>Explore our features</li>
                <li>Customize your settings</li>
            </ul>
            
            <p>If you have any questions, contact our support team at {supportEmail}.</p>
            
            <footer style='margin-top: 2rem; border-top: 1px solid #eee; padding-top: 1rem;'>
                <p>{_appSettings.ApplicationName} Team</p>
                <p><small>
                    <a href='{_appSettings.BaseUrl}/unsubscribe?email={recipientEmail}'>Unsubscribe</a> | 
                    {_appSettings.CompanyAddress}
                </small></p>
            </footer>";

                await SendEmailAsync(recipientEmail, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send registration email to {Email}", recipientEmail);
                // Consider implementing a retry mechanism here
            }
        }


        // Method for sending mails 
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

