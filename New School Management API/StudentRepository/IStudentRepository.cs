using New_School_Management_API.Data;
using New_School_Management_API.DTO;
using SchoolManagmentAPI.StudentDTO;
using New_School_Management_API.Entities;
using New_School_Management_API.StudentDTO;

namespace New_School_Management_API.Repository
{

    public interface IStudentRepository
    {
        Task<bool> creataStudentRecord(StudentRecord studentRecord);
        Task DeleteAsync(string studentMatricNumber);
        Task<List<StudentRecord>> GetStudentsByLevelAsync(int currentLevel, int skip, int take);

        Task<StudentRecord?> GetAsync(string studentMatricNumber);
        Task<StudentRecord?> GetByMatricNumberAsync(string studentMatricNumber);
        //Task<string> GetStudentAysnc (CheckResultDTO checkResult);
        IQueryable <StudentResponseClass> GetSpecifiRecordOfStudent(StudentResponseClass studentResponseClass);
        Task UpdateAsync(StudentRecord studentRecord);
        Task<bool> UserEmailAlreadyExist(string email);
        Task<bool> UserMatricNumberAlreadyExist(string studentMatricNumber);
        Task<int> GetTotalStudentsByLevelAsync(int currentLevel);
        Task<decimal> CheckResultAysnc(string studentMatricNumber);
        Task<bool> GpaExistsAsync(string studentMatricNumber);
    }
}
