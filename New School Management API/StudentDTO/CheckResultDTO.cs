using System.ComponentModel.DataAnnotations;

namespace SchoolManagmentAPI.StudentDTO
{
    public class CheckResultDTO
    {
        public string StudentName { get; set; }
        public int StudentMatricNumber { get; set; }
        public int Currentlevel { get; set; }

        [Required]
        public Double GPA { get; set; }
    }
}
