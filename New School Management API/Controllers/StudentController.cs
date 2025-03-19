using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using New_School_Management_API.ModelValidations;
using New_School_Management_API.StudentDTO;
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

        [HttpPut("Api/updateStudentRequest{studentMatnumber}")]
        [ValidationModelState]
        public async Task <IActionResult> UpdateStudentClass(string studenmatricNumber, [FromBody] UpdateStudentDTO updateStudentDTO)
        {
            await _serviceRepository.UpdateStudentRecords(studenmatricNumber,updateStudentDTO);
            return Ok();
        } 
    }
}
