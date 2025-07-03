namespace New_School_Management_API.Services.EmailService.EmailModel
{
    public class ApplicationSettings
    {
        public string ApplicationName { get; set; }

        public string LoginNotificationSubject { get; set; }

        public string BaseUrl { get; set; }

        public string SupportEmail { get; set; }

        public string SupportPhone { get; set; }

        public string CompanyAddress { get; set; }
    }
}
