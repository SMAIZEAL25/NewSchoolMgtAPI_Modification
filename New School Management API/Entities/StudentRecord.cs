using New_School_Management_API.Entities.Student_Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_School_Management_API.Entities
{
    public class StudentRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary Key (Guid)

        [Required]
        public required string StudentMatricNumber { get; set; }

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
        public string? StudentEmail { get; set; }

        public string? Faculty { get; set; }

        public string? Department { get; set; }

        [Required]
        public int Currentlevel { get; set; }

        [Phone]
        public long StudentPhoneNumber { get; set; }

        [Required]
        public double GPA { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        [Required, StringLength(200, ErrorMessage = "Your Password is Limited to {2} to {1} characters", MinimumLength = 6)]
        public required string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password do not match")]
        public required string ConfirmPassword { get; set; }

        // Linking the IdentityRole to StudentClass
        public string IdentityUserId { get; set; }

        // Navigation properties
        public ICollection<TransactionDetails> Transactions { get; set; }

        public ICollection<CourseRegistration> CourseRegistrations { get; set; }

        public ICollection<Upload> UploadedFiles { get; set; }
    }
}