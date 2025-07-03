using System.ComponentModel.DataAnnotations;

namespace New_School_Management_API.Domain.StudentDTO
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Your Password is Limited to {2} to {1} characters", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
