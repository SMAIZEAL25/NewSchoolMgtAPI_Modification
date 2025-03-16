
using New_School_Management_API.Data;
using New_School_Management_API.DTO;


namespace New_School_Management_API.StudentRepository
{
    public interface IServiceRepository
    { 
      Task<APIResponse> UpdateStudentRecords(UpdateStudentDTO updateStudsent);
      Task<APIResponse> GetStudentRecord (string StudentMatriNumber);
      Task<APIResponse> CheckResult(string StudentMatriNumber);
      Task<bool> Logout ();

      

    }
}
