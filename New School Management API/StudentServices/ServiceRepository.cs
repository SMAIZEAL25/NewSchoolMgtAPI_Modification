﻿
using AutoMapper;
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

        public ServiceRepository(IStudentRepository studentRepository, IMapper mapper, ILogger<ServiceRepository> logger)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<APIResponse<object>> UpdateStudentRecords(string studentMatricnumber, UpdateStudentDTO updateStudentDTO)
        {

            _logger.LogInformation($"Updating student records for {studentMatricnumber}");

            // Validate input
            if (string.IsNullOrEmpty(studentMatricnumber) || updateStudentDTO == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Please ensure the details entered are correct.");
                return _response;
            }

            try
            {
                // Fetch the existing record
                var existingRecord = await _studentRepository.GetAsync(studentMatricnumber);

                if (existingRecord == null)
                {
                    _logger.LogWarning($"Student record not found for {studentMatricnumber}");
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Student record not found.");
                    return _response;
                }

                // Map updated fields from DTO to existing entity
                _mapper.Map(updateStudentDTO, existingRecord);

                // Update the record in the repository
                await _studentRepository.UpdateAsync(existingRecord);

                _logger.LogInformation($"Student record updated successfully for {studentMatricnumber}");
                _response.IsSuccess = true;
                _response.Message = "Student record updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the student record for {studentMatricnumber}");
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("An error occurred while updating the student record.");
                _response.ErrorMessages.Add(ex.Message); // Include the exception message for debugging
            }

            return _response;
        }




        public async Task<PageResponse<GetStudentRecordDTO>> GetStudentsByLevelAsync(int currentLevel, int pageNumber, int pageSize)
        {
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
                StudentEmail = s.StudentEmail,
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
    

        public Task<APIResponse<object>> CheckResult(string studentMatricNumber)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
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