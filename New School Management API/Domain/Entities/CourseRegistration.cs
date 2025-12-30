using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace New_School_Management_API.Domain.Entities
{
    public class CourseRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public string Courses { get; set; }

        [Required]
        public string? CourseCode { get; set; }

        [ForeignKey(nameof(CourseCode))]
        public Course? Course { get; set; }

        [Required]
        public int Credit { get; set; } 

        public DateTime RegistrationDate { get; set; }

        // Navigation property to StudentRecord
        [ForeignKey(nameof(StudentId))]
        public StudentRecord? Student { get; set; }

        
    }
}