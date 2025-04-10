namespace New_School_Management_API.Data
{
    public class AuthResponseDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresIn { get; set; }
        public List<string> Roles { get; set; }


    }
}
