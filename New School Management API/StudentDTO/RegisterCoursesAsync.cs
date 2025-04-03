using System.ComponentModel.DataAnnotations;

namespace New_School_Management_API.StudentDTO
{
    public class RegisterCoursesDTO
    {
        
        public int CurrentLevel { get; set; }
        public string CourseCode { get; set; } // Primary key
        public string CourseName { get; set; }

    }
}
