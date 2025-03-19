using System.ComponentModel.DataAnnotations;

namespace SchoolManagmentAPI.StudentDTO
{
    public class CheckResultDTO
    {
        public string StudentName { get; set; }
        public string StudentMatricNumber { get; set; }
        public int Currentlevel { get; set; }

        [Required]
        public Double GPA { get; set; }
    }
}
