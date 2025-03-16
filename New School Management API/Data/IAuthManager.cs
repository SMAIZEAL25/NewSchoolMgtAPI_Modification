using AutoMapper;
using Microsoft.AspNetCore.Identity;
using New_School_Management_API.DTO;
using New_School_Management_API.StudentDTO;

namespace New_School_Management_API.Data
{
    public interface IAuthManager
    {

     
        Task<APIResponse> Register(CreateStudentDTO createStudentDTO, string role = "User");

        Task <APIResponse> Login(LoginDTO loginDTO);
    }
}
