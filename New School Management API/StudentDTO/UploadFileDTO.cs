using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace New_School_Management_API.StudentDTO
{
    public class UploadFileDTO
    {
      [Required]
      public IFormFile File { get; set; }
       
      [Required]
      public string Filename { get; set; } 
      public string? FileDescription { get; set; }
    }
}
