using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace New_School_Management_API.DTO
{
    public class GetStudentRecordDTO
    {
        public required string SurName { get; set; }

        [Required]
        [MaxLength(30)]
        public required string MiddleName { get; set; }

        [Required]
        [MaxLength(30)]
        public required string LastName { get; set; }
        public string Sex { get; set; }
        public required string StudentEmail { get; set; }
        public int Currentlevel { get; set; }
        public string StudentPhoneNumber { get; set; }
        public required string StudentMatriNumber { get; set; }
        public Decimal GPA { get; set; }
        public string? Faculty { get; set; }
        public string? Department { get; set; }

    }
}
