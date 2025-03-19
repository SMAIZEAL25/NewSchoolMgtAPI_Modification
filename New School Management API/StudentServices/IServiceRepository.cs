
using New_School_Management_API.Data;
using New_School_Management_API.DTO;
using New_School_Management_API.StudentDTO;


namespace New_School_Management_API.StudentRepository
{
    public interface IServiceRepository
    { 
      Task<APIResponse> UpdateStudentRecords(string studentMatricnumber, UpdateStudentDTO updateStudentDTO);
      Task<APIResponse> CheckResult(string StudentMatriNumber);
      Task<bool> Logout ();
      Task<APIResponse> GetStudentRecord(GetStudentRecordDTO getStudentRecordDTO);
    }
}
