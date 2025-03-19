using New_School_Management_API.Data;
using New_School_Management_API.DTO;
using SchoolManagmentAPI.StudentDTO;
using New_School_Management_API.Entities;

namespace New_School_Management_API.Repository
{

    public interface IStudentRepository
    {
        Task<bool> AddAsync(StudentRecord studentRecord);
        Task DeleteAsync(string studentMatricNumber);
        Task<List<GetStudentRecordDTO>> GetAllAsync(GetStudentRecordDTO getStudentRecordDTO);
        Task<StudentRecord?> GetAsync(string studentMatricNumber);
        Task<StudentRecord?> GetByMatricNumberAsync(string studentMatricNumber);
        Task<StudentRecord?> GetPasswordAsync(string passwordHash);
        Task<string> GetStudentResult(CheckResultDTO checkResult);
        Task<bool> GetUserNameAsync(string username);
        Task UpdateAsync(StudentRecord studentRecord);
        Task<bool> UserEmailAlreadyExist(string email);
        Task<bool> UserMatricNumberAlreadyExist(string studentMatricNumber);
    }
}
