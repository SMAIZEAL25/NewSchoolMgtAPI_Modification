using System.ComponentModel.DataAnnotations;

namespace SchoolManagmentAPI.StudentDTO
{
    public class CheckResultDTO
    {
        public string Surname { get; set; }
        public string StudentMatricNumber { get; set; }
        public int Currentlevel { get; set; }

    }
}
