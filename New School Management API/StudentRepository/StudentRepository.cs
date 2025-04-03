
using Microsoft.EntityFrameworkCore;
using New_School_Management_API.Data;
using New_School_Management_API.Dbcontext;
using New_School_Management_API.DTO;
using New_School_Management_API.Entities;
using New_School_Management_API.StudentDTO;
using SchoolManagmentAPI.StudentDTO;


namespace New_School_Management_API.Repository
{
    public class StudentRepository : IStudentRepository
    {

        private readonly StudentManagementDB _dBContext;


        public StudentRepository(StudentManagementDB dBContext)
        {
            _dBContext = dBContext;

        }

        public async Task<bool> creataStudentRecord (StudentRecord studentRecord)
        {
            await _dBContext.AddAsync(studentRecord);
            await _dBContext.SaveChangesAsync();
            return true;
            
        }

        public async Task<StudentRecord?> GetAsync(string studentMatricNumber)
        {
            return await _dBContext.StudentRecords.FindAsync(studentMatricNumber);
        }

        public async Task<List<StudentRecord>> GetStudentsByLevelAsync(int currentLevel, int skip, int take)
        {
            return await _dBContext.StudentRecords
                .Where(s => s.Currentlevel == currentLevel)
                .OrderBy(s => s.StudentMatricNumber)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> GetTotalStudentsByLevelAsync(int currentLevel)
        {
            return await _dBContext.StudentRecords
                .CountAsync(s => s.Currentlevel == currentLevel);
        }


        public async Task<StudentRecord?> GetByMatricNumberAsync(string studentMatricNumber)
        {
            if (string.IsNullOrWhiteSpace(studentMatricNumber))
            {
                return null;
            }
            return await _dBContext.StudentRecords
                .FirstOrDefaultAsync(s => s.StudentMatricNumber == studentMatricNumber);
        }



        public async Task UpdateAsync(StudentRecord studentRecord)
        {
            _dBContext.StudentRecords.Update(studentRecord);
            await _dBContext.SaveChangesAsync();
        }



        public async Task DeleteAsync(string studentMatricNumber)
        {
            var studentToDelete = await _dBContext.StudentRecords
                .FirstOrDefaultAsync(s => s.StudentMatricNumber == studentMatricNumber);

            if (studentToDelete == null)
                throw new KeyNotFoundException("Student not found.");

            _dBContext.StudentRecords.Remove(studentToDelete);
            await _dBContext.SaveChangesAsync();
        }

        //public async Task<string> GetStudentAsync (CheckResultDTO checkResult)
        //{
        //    var student = await _dBContext.StudentRecords
        //        .FirstOrDefaultAsync(s => s.StudentMatricNumber == checkResult.StudentMatricNumber);

        //    return "StudentResult Successful";
        //}

        public async Task<bool> UserEmailAlreadyExist(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            return await _dBContext.StudentRecords.AnyAsync(u => u.StudentEmail == email);
        }

        public IQueryable<StudentResponseClass> GetSpecifiRecordOfStudent(StudentResponseClass studentResponseClass)
        {
            return  _dBContext.StudentRecords.Where(u => u.StudentMatricNumber == studentResponseClass.StudentMatricNumber)
                .Select(u => new StudentResponseClass
            {
                StudentMatricNumber = u.StudentMatricNumber,
                SurName = u.SurName,
                MiddleName = u.MiddleName,
                LastName = u.LastName,
                Department = u.Department,
                CurrentLevel = u.Currentlevel,
            });

        }

        //public async Task<StudentRecord?> GetPasswordAsync(string passwordHash)
        //{
        //    if (string.IsNullOrEmpty(passwordHash))
        //    {
        //        return null;
        //    }

        //    return await _dBContext.StudentRecords
        //        .SingleOrDefaultAsync(u => u.Password == passwordHash);
        //}

        public async Task<bool> UserMatricNumberAlreadyExist(string studentMatricNumber)
        {
            return await _dBContext.StudentRecords.AnyAsync(u => u.StudentMatricNumber == studentMatricNumber);
        }
  
        public async Task<decimal>  CheckResultAysnc(string studentMatricNumber)
        {
            return await _dBContext.StudentRecords
               .Where(x => x.StudentMatricNumber == studentMatricNumber)
               .Select(x => x.GPA)
               .FirstOrDefaultAsync();
        }

        public async Task<bool> GpaExistsAsync(string studentMatricNumber)
        {
            return await _dBContext.StudentRecords
                .AnyAsync(s => s.StudentMatricNumber == studentMatricNumber && s.GPA != null);
        }

        public async Task<string?> GetLastMatricNumber(string deptCode, string facCode, string year)
        {
            
            var matricNumbers = await _dBContext.StudentRecords
                .Where(s => s.StudentMatricNumber.StartsWith($"{deptCode}/{facCode}/{year}/"))
                .OrderByDescending(s => s.StudentMatricNumber)
                .Select(s => s.StudentMatricNumber)
                .ToListAsync();

            return matricNumbers.FirstOrDefault();
        }

        //public Task<string> GetStudentAysnc(CheckResultDTO checkResult)
        //{
        //    throw new NotImplementedException();
        //}


    }

}
