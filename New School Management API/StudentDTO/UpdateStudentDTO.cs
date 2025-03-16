using System.ComponentModel.DataAnnotations;

namespace New_School_Management_API.DTO
{
    public class UpdateStudentDTO
    {

        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public int Currentlevel { get; set; }
        public int StudentPhoneNumber { get; set; }
        [Required]
        public string StudentMatricNumber { get; set; }
    }
}
