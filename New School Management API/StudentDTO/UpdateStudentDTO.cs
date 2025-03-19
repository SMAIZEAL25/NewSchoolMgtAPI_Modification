using System.ComponentModel.DataAnnotations;

namespace New_School_Management_API.StudentDTO
{
    public class UpdateStudentDTO
    {

        [Required]
        [MaxLength(30)]
        public required string SurName { get; set; }

        [Required]
        [MaxLength(30)]
        public required string MiddleName { get; set; }

        [Required]
        [MaxLength(30)]
        public required string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string StudentEmail { get; set; }
        public int Currentlevel { get; set; }
        public int StudentPhoneNumber { get; set; }
    
    }
}
