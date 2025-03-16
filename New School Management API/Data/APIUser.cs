﻿using Microsoft.AspNetCore.Identity;

namespace New_School_Management_API.Data
{
    public class APIUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }
    }
}
