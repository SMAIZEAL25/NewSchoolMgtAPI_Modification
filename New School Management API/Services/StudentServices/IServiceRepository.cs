using New_School_Management_API.Domain.Data;
using New_School_Management_API.Domain.StudentDTO;
using New_School_Management_API.PagInated_Response;


namespace New_School_Management_API.Services.StudentServices
{
    public interface IServiceRepository
    {
        Task<APIResponse<object>> UpdateStudentRecords(string studentMatricnumber, UpdateStudentDTO updateStudentDTO);
        Task<StudentResponseClass> GetStudentResultAsync(string studentMatricNumber, bool isLoggedIn);
        Task<PageResponse<GetStudentRecordDTO>> GetStudentsByLevelAsync(int currentLevel, int pageNumber, int pageSize);
    }
}
