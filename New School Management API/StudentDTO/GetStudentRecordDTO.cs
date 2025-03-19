using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace New_School_Management_API.DTO
{
    public class GetStudentRecordDTO
    {
        public string StudentName { get; set; }
        public string Sex { get; set; }
        public string StudentEmail { get; set; }
        public int Currentlevel { get; set; }
        public int StudentPhoneNumber { get; set; }
        public string StudentMatriNumber { get; set; }
        public Double GPA { get; set; }
        public string? Faculty { get; set; }
        public string? Department { get; set; }

    }
}
