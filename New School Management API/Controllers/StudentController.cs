using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using New_School_Management_API.DTO;
using New_School_Management_API.Repository;
using New_School_Management_API.StudentRepository;

namespace New_School_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;

       public StudentController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        [HttpPut("Api/updateStudentRequest")]
        public async Task <IActionResult> UpdateStudentClass(UpdateStudentDTO updateStudentDTO)
        {
            await _serviceRepository.UpdateStudentRecords(updateStudentDTO);
            return Ok();

        } 
    }
}
