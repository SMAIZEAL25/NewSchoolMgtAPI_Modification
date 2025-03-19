using AutoMapper;
using Microsoft.EntityFrameworkCore;
using New_School_Management_API.Dbcontext;
using New_School_Management_API.Entities;
using SchoolManagmentAPI.StudentDTO;
using New_School_Management_API.DTO;
using New_School_Management_API.Repository;

namespace New_School_Management_API.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentManagementDB _dBContext;
        private readonly IMapper _mapper;

        public StudentRepository(StudentManagementDB dBContext, IMapper mapper)
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(StudentRecord studentRecord)
        {
            await _dBContext.AddAsync(studentRecord);
            await _dBContext.SaveChangesAsync();
            return true;
        }

        public async Task<StudentRecord?> GetAsync(string studentMatricNumber)
        {
            return await _dBContext.StudentRecords.FindAsync(studentMatricNumber);
        }

        public async Task<List<GetStudentRecordDTO>> GetAllAsync(GetStudentRecordDTO getStudentRecordDTO)
        {
            var studentRecords = await _dBContext.StudentRecords.ToListAsync();
            return _mapper.Map<List<GetStudentRecordDTO>>(studentRecords);
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

        public async Task<string> GetStudentResult(CheckResultDTO checkResult)
        {
            var student = await _dBContext.StudentRecords
                .FirstOrDefaultAsync(s => s.StudentMatricNumber == checkResult.StudentMatricNumber);

            return "StudentResult Successful";
        }

        public async Task<bool> UserEmailAlreadyExist(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            return await _dBContext.StudentRecords.AnyAsync(u => u.StudentEmail == email);
        }

        public async Task<bool> GetUserNameAsync(string username)
        {
            return await _dBContext.StudentRecords.AnyAsync(u => u.StudentMatricNumber == username);
        }

        public async Task<StudentRecord?> GetPasswordAsync(string passwordHash)
        {
            if (string.IsNullOrEmpty(passwordHash))
            {
                return null;
            }

            return await _dBContext.StudentRecords
                .SingleOrDefaultAsync(u => u.Password == passwordHash);
        }

        public async Task<bool> UserMatricNumberAlreadyExist(string studentMatricNumber)
        {
            return await _dBContext.StudentRecords.AnyAsync(u => u.StudentMatricNumber == studentMatricNumber);
        }
    }
}