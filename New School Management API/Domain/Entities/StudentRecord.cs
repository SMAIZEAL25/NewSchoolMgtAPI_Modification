using New_School_Management_API.Entities.Student_Transaction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_School_Management_API.Domain.Entities
{
    public class StudentRecord
    {
        [Key]
        public int Id { get; set; } // Primary Key (Guid)

        [Required]
        public string StudentMatricNumber { get; set; }

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

        [Required]
        public required string Sex { get; set; }

        [Required]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$")]
        public string? StudentEmailAddress { get; set; }

        public string? Faculty { get; set; }

        public string? Department { get; set; }

        [Required]
        public int Currentlevel { get; set; }

        public required string State_of_Origin { get; set; }

        public string Local_Goverment_Of_Origin { get; set; }

        [Phone]
        public string StudentPhoneNumber { get; set; }

        [Required]
        public decimal GPA { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        // Linking the IdentityUser to StudentClass
        public string IdentityUserId { get; set; }

        public int Transaction_Id { get; set; }

        public int CourseRgistration_Id { get; set; }

        // Navigation properties
        //public ICollection<TransactionDetails> Transactions { get; set; }

        //public ICollection<CourseRegistration> CourseRegistrations { get; set; }

        //public ICollection<Upload> UploadedFiles { get; set; }

        public ICollection<TransactionDetails> Transactions { get; set; } = new List<TransactionDetails>();
        public ICollection<CourseRegistration> CourseRegistrations { get; set; } = new List<CourseRegistration>();
        public ICollection<Upload> UploadedFiles { get; set; } = new List<Upload>();
    }
}