using Microsoft.AspNetCore.Identity;

namespace New_School_Management_API.Domain.Data
{
    public class APIUser : IdentityUser
    {

        public string Email { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }
    }
}
