using New_School_Management_API.Data;
using New_School_Management_API.DTO;
using SchoolManagmentAPI.StudentDTO;
using New_School_Management_API.Entities;


namespace New_School_Management_API.Repository
{

    public interface IStudentRepository 
    {
        Task <bool> AddAsync(StudentRecord studentRecord );
        public Task<bool> UpdateAsync(UpdateStudentDTO updateStudentDTO);
        Task  DeleteAsync (string StudentMatricNumber);
        public Task<List<GetStudentRecordDTO>> GetAllAsync(string studentMatricNumber);
        public Task<StudentRecord?> GetByMatericNumberAsync(string StudentMatricNumber);
        public Task<string> GetStudenResult(CheckResultDTO checkResult);
        public Task<bool> UserEmailAlreadyExist(string Email);
        public Task<bool> GetAsync(string StudentMatricNumber);
        public Task<StudentRecord> GetpasswordAsync(string passwordHash);
        public Task<bool> UserMatricNumberAlreadyExist(string studentMatricNumber);
        Task<bool> GetUserNameAysnc(string Username);
        //Task<StudentTimetable> FetchClassTimeTableAysnc(int CurrentLevel);
        //Task<StudentTimetable> FetchExamTimeTableAysnc(int CurrentLevel);
        //Task<string> SeedTimeTableDataAsync(List<ScheduleItem> ClassTimetable, List<ScheduleItem> ExamTimeTable);
       
        
    }
}
