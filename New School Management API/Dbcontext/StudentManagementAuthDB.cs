using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using New_School_Management_API.Data;

namespace New_School_Management_API.Dbcontext
{
    public class StudentManagementAuthDB : IdentityDbContext
    {
        public StudentManagementAuthDB(DbContextOptions<StudentManagementAuthDB> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerId = "d2efe1ee-1bb7-40be-bfe1-35794c090a9e";
            var WriteId = "188b9110-d56e-4a76-9e42-5469dc2070b1";

            var reles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerId,
                    ConcurrencyStamp = readerId,
                    Name = "Reader",
                    NormalizedName = "READER".ToUpper()
                },
                new IdentityRole
                {
                    Id = WriteId,
                    ConcurrencyStamp = WriteId,
                    Name = "Writer",
                    NormalizedName = "WRITER".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(reles);
        }
    }
}
