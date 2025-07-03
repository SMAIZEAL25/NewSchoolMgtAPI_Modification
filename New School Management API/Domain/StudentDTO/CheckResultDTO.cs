using System.ComponentModel.DataAnnotations;

namespace New_School_Management_API.Domain.StudentDTO
{
    public class CheckResultDTO
    {
        public string Surname { get; set; }
        public string StudentMatricNumber { get; set; }
        public int Currentlevel { get; set; }

    }
}
