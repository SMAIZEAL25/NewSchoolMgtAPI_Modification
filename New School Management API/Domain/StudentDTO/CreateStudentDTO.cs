using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace New_School_Management_API.Domain.StudentDTO
{
    public class CreateStudentDTO
    {
        [Required]
        [MaxLength(30)]
        public required string SurName { get; set; }

        [Required]
        [MaxLength(30)]
        public string MiddleName { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public required string LastName { get; set; }

        [Required]
        public required string Sex { get; set; }

        [Required]
        public int Currentlevel { get; set; }

        [Phone]
        public string StudentPhoneNumber { get; set; }

        public required string StudentMatricNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        public required string StudentEmailAddress { get; set; }

        public required string State_of_Origin { get; set; }

        public string Local_Goverment_Of_Origin { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Confrim Password is required")]
        [Compare("Password", ErrorMessage = "Paasword do not match")]

        public required string ConfirmPassword { get; set; }
        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public string? Roles { get; set; }



    }
}

