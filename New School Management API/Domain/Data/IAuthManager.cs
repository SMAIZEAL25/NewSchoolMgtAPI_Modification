using AutoMapper;
using Microsoft.AspNetCore.Identity;
using New_School_Management_API.Domain.StudentDTO;

namespace New_School_Management_API.Domain.Data
{
    public interface IAuthManager
    {
        Task<APIResponse<object>> Register(CreateStudentDTO createStudentDTO);

        Task<APIResponse<object>> Login(LoginDTO loginDTO);
    }
}
