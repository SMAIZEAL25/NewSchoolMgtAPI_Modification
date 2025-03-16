
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace New_School_Management_API.DTO
{
    public class CreateStudentDTO
    {
        [Required]
        [MaxLength(30)]
        public string SurName { get; set; }

        [Required]
        [MaxLength(30)]
        public string MiddleName { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        public string Sex { get; set; }
        
        [Required]
        public int Currentlevel { get; set; }
        [Phone]
        public int StudentPhoneNumber { get; set; }

        [Required]
        public string StudentMatricNumber { get; set; }

        [Required]
        public string StudentEmailAddress { get; set; }

        [Required, StringLength(2000000, ErrorMessage =
        "Your Password is Limited to {2} to {i} characters", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confrim Password is required")]
        [Compare("Password", ErrorMessage = "Paasword do not match")]
        public string ConfirmPassword { get; set; }


    }
}

