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
        private readonly ILogger<StudentController> _logger;

        public StudentController(IServiceRepository serviceRepository, ILogger <StudentController> logger)
        {
            _serviceRepository = serviceRepository;
           _logger = logger;
        }

        [HttpPut("/updateStudentRequest{studentMatnumber}")]
        [ValidationModelState]
        public async Task <IActionResult> UpdateStudentClass(string studenmatricNumber, [FromBody] UpdateStudentDTO updateStudentDTO)
        {
            await _serviceRepository.UpdateStudentRecords(studenmatricNumber,updateStudentDTO);
            return Ok();
        }


        [HttpGet("/GetstudentRecordsByCurrentLevel")]

        public async Task<IActionResult> GetStudentsByCurrentLevel(int currentLevel, int pageNumber = 1, int pageSize = 20)
        {
            _logger.LogInformation($"Fetching students for level {currentLevel}, page {pageNumber}, page size {pageSize}");

            // Validate the current level    Validate the page number and page size
            if (currentLevel < 100 || currentLevel > 400 && pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Current level must be between 100 and 400, \"Page number and page size must be greater than 0.\"");
            }

            // Validate the page number and page size
            //if (pageNumber < 1 || pageSize < 1)
            //{
            //    return BadRequest("Page number and page size must be greater than 0.");
            //}

            try
            {
                // Call the service method to get paginated students
                var response = await _serviceRepository.GetStudentsByLevelAsync(currentLevel, pageNumber, pageSize);

                if (response.Data == null || !response.Data.Any())
                {
                    return NotFound("No students found for the specified level and page.");
                }

                // Return the paginated list of students with metadata
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching students for level {currentLevel}");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
    }
 }

