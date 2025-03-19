
using New_School_Management_API.Data;
using New_School_Management_API.DTO;
using New_School_Management_API.PagInated_Response;
using New_School_Management_API.StudentDTO;


namespace New_School_Management_API.StudentRepository
{
    public interface IServiceRepository
    { 
      Task<APIResponse<object>> UpdateStudentRecords(string studentMatricnumber, UpdateStudentDTO updateStudentDTO);
      Task<APIResponse<object>> CheckResult(string StudentMatriNumber);
      Task<bool> Logout ();
      Task<PageResponse<GetStudentRecordDTO>> GetStudentsByLevelAsync(int currentLevel, int pageNumber, int pageSize);
    }
}
