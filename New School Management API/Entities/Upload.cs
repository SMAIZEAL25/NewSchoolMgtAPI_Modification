using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace New_School_Management_API.Entities
{
    public class Upload
    {
        [Key]
        public Guid UploadfileId { get; set; }

        public int StudentId { get; set; } // Foreign Key to StudentRecord (int)

        [NotMapped]
        public IFormFile file { get; set; } // Not mapped to the database

        public string FileName { get; set; }

        public string fileExtension { get; set; } // e.g., "JPG" or "PDF"

        public long FileSizeInBytes { get; set; } // File size in bytes

        public string filePath { get; set; }

        public DateTime UploadedOn { get; set; }

        public byte[] FileDescription { get; set; } // File content as byte array

        // Navigation property to StudentRecord
        [ForeignKey(nameof(StudentId))]
        public StudentRecord Student { get; set; }
    }
}