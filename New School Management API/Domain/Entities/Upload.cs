using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using New_School_Management_API.Migrations.SchoolMgtAuthDbMigrations;

namespace New_School_Management_API.Domain.Entities
{
    public class Upload
    {
        public int Id { get; set; }

        [NotMapped]
        public IFormFile file { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(500)]
        public string? FileDescription { get; set; }

        [StringLength(10)]
        public string FileExtension { get; set; }

        [Required]
        [StringLength(500)]
        public long FileSizeInBytes { get; set; }

        public string FilePath { get; set; }

        //Navigation property to StudentRecord

        public int StudentRecordId { get; set; }

        [ForeignKey(nameof(StudentRecordId))]
        public StudentRecord Student { get; set; }
    }
}