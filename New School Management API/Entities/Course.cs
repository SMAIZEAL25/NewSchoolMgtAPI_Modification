using System.ComponentModel.DataAnnotations;

namespace New_School_Management_API.Entities
{
    public class Course
    {
        [Key]
        public string CourseCode { get; set; } // Primary key

        [Required]
        public string CourseName { get; set; }

        // Navigation property for CourseRegistrations
        public ICollection<CourseRegistration> CourseRegistrations { get; set; } = new List<CourseRegistration>();
    }
}
