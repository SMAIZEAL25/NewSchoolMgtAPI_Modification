using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_School_Management_API.Entities
{
    public class CourseRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; } // Changed from string to int to match StudentRecord.Id

        [Required]
        public string Courses { get; set; } // Comma-separated list of courses

        public DateTime RegistrationDate { get; set; }

        // Navigation property to StudentRecord
        [ForeignKey(nameof(StudentId))]
        public StudentRecord Student { get; set; }

        public string CourseCode { get; set; }

        [ForeignKey(nameof(CourseCode))]
        public Course Course { get; set; }
    }
}