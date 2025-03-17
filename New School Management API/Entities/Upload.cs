using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace New_School_Management_API.Entities
{
    public class Upload
    {
        public Guid Id { get; set; }

        [NotMapped]
        public IFormFile file { get; set; }

        public string FileName { get; set; }

        public string? FileDescription { get; set; }

        public string FileExtension { get; set; }

        public long FileSizeInBytes { get; set; }

        public string FilePath { get; set; }

        // Navigation property to StudentRecord
        //[ForeignKey(nameof(StudentId))]
        //public StudentRecord Student { get; set; }
    }
}