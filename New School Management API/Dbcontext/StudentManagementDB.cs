using Microsoft.EntityFrameworkCore;
using New_School_Management_API.Entities;
using New_School_Management_API.Entities.Student_Transaction;

namespace New_School_Management_API.Dbcontext
{
    public class StudentManagementDB : DbContext
    {
        public StudentManagementDB(DbContextOptions<StudentManagementDB> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<StudentRecord> StudentRecords { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseRegistration> CourseRegistrations { get; set; }
        public DbSet<Upload> Uploads { get; set; }
        public DbSet<TransactionDetails> Transactions { get; set; } // Fixed naming

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Configure StudentRecord -> CourseRegistrations relationship
    modelBuilder.Entity<StudentRecord>()
        .HasMany(s => s.CourseRegistrations)
        .WithOne(cr => cr.Student)
        .HasForeignKey(cr => cr.StudentId);

    // Configure Course -> CourseRegistrations relationship
    modelBuilder.Entity<Course>()
        .HasMany(c => c.CourseRegistrations)
        .WithOne(cr => cr.Course)
        .HasForeignKey(cr => cr.CourseCode);

    //// Configure Upload -> StudentRecord relationship
    //modelBuilder.Entity<Upload>()
    //    .HasOne(u => u.Student)
    //    .WithMany(s => s.UploadedFiles)
    //    .HasForeignKey(u => u.StudentId);

    // Configure TransactionDetails -> StudentRecord relationship
    modelBuilder.Entity<TransactionDetails>()
        .HasOne(t => t.Student)
        .WithMany(s => s.Transactions)
        .HasForeignKey(t => t.StudentId)
        .OnDelete(DeleteBehavior.Cascade);

    // Configure precision for decimal properties in TransactionDetails
    modelBuilder.Entity<TransactionDetails>()
        .Property(t => t.Amount)
        .HasPrecision(18, 2);

    modelBuilder.Entity<TransactionDetails>()
        .Property(t => t.VAT)
        .HasPrecision(18, 2);
}
    }
}
