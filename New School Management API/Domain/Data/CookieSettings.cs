namespace New_School_Management_API.Domain.Data
{
    public class CookieSettings
    {

        public string Name { get; set; }
        public string Value { get; set; }
        public int MaxAge { get; set; }
        public bool HttpOnly { get; set; }
        public bool Secure { get; set; }
        public string SameSite { get; set; }

    }

}
