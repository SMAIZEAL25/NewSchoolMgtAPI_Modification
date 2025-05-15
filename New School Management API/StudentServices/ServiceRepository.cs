
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using New_School_Management_API.Data;
using New_School_Management_API.DTO;
using New_School_Management_API.PagInated_Response;
using New_School_Management_API.Repository;
using New_School_Management_API.StudentDTO;


namespace New_School_Management_API.StudentRepository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly APIResponse <object> _response = new APIResponse<object>();
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<ServiceRepository> _logger;
        private readonly IMemoryCache _memoryCache;

        public ServiceRepository(IStudentRepository studentRepository, 
            IMapper mapper, 
            ILogger<ServiceRepository> logger,
            IMemoryCache memoryCache
            )
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<APIResponse<object>> UpdateStudentRecords(string studentMatricnumber, UpdateStudentDTO updateStudentDTO)
        {
            // Initialize a new response for this request
            var response = new APIResponse<object>();

            _logger.LogInformation($"Updating student records for {studentMatricnumber}");

            // Validate input
            if (string.IsNullOrEmpty(studentMatricnumber) || updateStudentDTO == null)
            {
                response.IsSuccess = false;
                response.ErrorMessages.Add("Please ensure the details entered are correct.");
                return response;
            }

            try
            {
                var existingRecord = await _studentRepository.GetAsync(studentMatricnumber);

                if (existingRecord == null)
                {
                    _logger.LogWarning($"Student record not found for {studentMatricnumber}");
                    response.IsSuccess = false;
                    response.ErrorMessages.Add("Student record not found.");
                    return response;
                }

                _mapper.Map(updateStudentDTO, existingRecord);
                await _studentRepository.UpdateAsync(existingRecord);

                _logger.LogInformation($"Student record updated successfully for {studentMatricnumber}");
                response.IsSuccess = true;
                response.Message = "Student record updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the student record for {studentMatricnumber}");
                response.IsSuccess = false;
                response.ErrorMessages.Add("An error occurred while updating the student record.");
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }





        public async Task<PageResponse<GetStudentRecordDTO>> GetStudentsByLevelAsync(int currentLevel, int pageNumber, int pageSize)
        {

            if (currentLevel < 100 || currentLevel > 400 && pageNumber < 1 || pageSize < 1)
            {
                return new PageResponse<GetStudentRecordDTO>
                {
                    ErrorMessage = ("Current level must be between 100 and 400, \"Page number and page size must be greater than 0.\"") 
                };
            }
            // Calculate the number of records to skip
            int skip = (pageNumber - 1) * pageSize;

            // Fetch a paginated list of students
            var students = await _studentRepository.GetStudentsByLevelAsync(currentLevel, skip, pageSize);

            // Get the total number of records
            int totalRecords = await _studentRepository.GetTotalStudentsByLevelAsync(currentLevel);

            // Calculate the total number of pages
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Map the result to DTO
            var studentDTOs = students.Select(s => new GetStudentRecordDTO
            {
                StudentMatriNumber = s.StudentMatricNumber,
                SurName = s.SurName,
                MiddleName = s.MiddleName,
                LastName = s.LastName,
                StudentEmail = s.StudentEmailAddress,
                Currentlevel = s.Currentlevel,
                Faculty = s.Faculty,
                Department = s.Department,
                StudentPhoneNumber = s.StudentPhoneNumber,
                Sex = s.Sex,
                GPA = s.GPA,
            }).ToList();

            // Create the response with metadata
            return new PageResponse<GetStudentRecordDTO>
            {
                Data = studentDTOs,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }



        public async Task<StudentResponseClass> GetStudentResultAsync(string studentMatricNumber, bool isLoggedIn)
        {
            try
            {
                _logger.LogInformation($"Fetching result for student: {studentMatricNumber}");

                if (isLoggedIn)
                {
                    // Check cache first
                    if (_memoryCache.TryGetValue($"GPA_{studentMatricNumber}", out decimal cachedGpa))
                    {
                        _logger.LogInformation($"Returning cached GPA for {studentMatricNumber}");
                        return new StudentResponseClass { GPA = cachedGpa };
                    }

                    // Fetch from DB if not cached
                    var gpaExists = await _studentRepository.GpaExistsAsync(studentMatricNumber);
                    if (gpaExists)
                    {
                        var gpa = await _studentRepository.CheckResultAysnc(studentMatricNumber); // Changed to GetGpaAsync
                        _memoryCache.Set($"GPA_{studentMatricNumber}", gpa, TimeSpan.FromMinutes(5));
                        _logger.LogInformation($"Updated cache for {studentMatricNumber}");
                        return new StudentResponseClass { GPA = gpa };
                    }
                }
                else
                {
                    // For non-logged-in students - get first matching record
                    var studentDetails = await _studentRepository.GetSpecifiRecordOfStudent(new StudentResponseClass { StudentMatricNumber = studentMatricNumber })
                       .FirstOrDefaultAsync(); // Added FirstOrDefaultAsync

                    if (studentDetails != null)
                    {
                        return new StudentResponseClass
                        {
                            SurName = studentDetails.SurName,
                            MiddleName = studentDetails.MiddleName,
                            LastName = studentDetails.LastName,
                            Department = studentDetails.Department,
                            StudentMatricNumber = studentDetails.StudentMatricNumber,
                            CurrentLevel = studentDetails.CurrentLevel,
                            GPA = studentDetails.GPA
                        };
                    }
                }

                _logger.LogWarning($"No data found for student: {studentMatricNumber}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching result for {studentMatricNumber}");
                throw;
            }
        }



     
        //public async Task<APIResponse> GetTimeTableAsync(int level)
        //{
        //    _logger.LogInformation("Fetching timetable for level {Level}", level);

        //    if (level < 100)
        //    {
        //        _logger.LogWarning("Invalid student level: {Level}", level);
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages.Add("Invalid student level. Please enter your correct class.");
        //        return _response;
        //    }

        //    var classTimetable = await _studentRepository.FetchClassTimeTableAysnc(level);
        //    var examTimetable = await _studentRepository.FetchExamTimeTableAysnc(level);

        //    _logger.LogInformation("Timetable fetched successfully for level {Level}", level);
        //    _response.IsSuccess = true;
        //    _response.Result = new { ClassTimetable = classTimetable, ExamTimetable = examTimetable };
        //    return _response;
        //}
    }
}