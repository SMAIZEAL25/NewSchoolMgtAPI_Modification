using New_School_Management_API.StudentDTO;

namespace New_School_Management_API.Services.EmailService
{
    public interface IEmailService
    {
        Task SendLoginNotificationAsync(string recipientEmail, string ipAddress, DateTime loginTime);
        Task SendEmailAsync(string recipientEmail, string subject, string body);
        Task SendRegistrationSuccessEmailAsync(string recipientEmail, string username);

    }
}
