
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using New_School_Management_API.Data;
using New_School_Management_API.DTO;
using New_School_Management_API.Entities;
using New_School_Management_API.Repository;
using System.Text.RegularExpressions;

namespace New_School_Management_API.StudentRepository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly APIResponse _response = new APIResponse();
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<ServiceRepository> _logger;

        public ServiceRepository(IStudentRepository studentRepository, IMapper mapper, ILogger<ServiceRepository> logger)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<APIResponse> UpdateStudentRecords(UpdateStudentDTO updateStudent)
        {
      
            // Log the start of the operation
            _logger.LogInformation($"Updating student records for {updateStudent?.StudentName}");

            // Validate the input
            if (updateStudent == null || string.IsNullOrEmpty(updateStudent.StudentMatricNumber))
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Please ensure the details entered are correct.");
                return _response;
            }

            try
            {
                // Fetch the existing record from the repository
                var existingRecord = await _studentRepository.GetAsync(updateStudent.StudentMatricNumber);

                // Check if the record exists
                if (existingRecord == null)
                {
                    _logger.LogWarning($"Student record not found for {updateStudent.StudentMatricNumber}");
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Student record not found.");
                    return _response;
                }

                // Map the updated fields from the DTO to the existing entity
                 _mapper.Map(updateStudent, existingRecord);

                // Update the record in the repository
                await _studentRepository.UpdateAsync(existingRecord);

                // Log the successful update
                _logger.LogInformation($"Student record updated successfully for {updateStudent.StudentMatricNumber}");

                // Set the response
                _response.IsSuccess = true;
                _response.Message = "Student record updated successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, $"An error occurred while updating the student record for {updateStudent.StudentMatricNumber}");

                // Set the response
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("An error occurred while updating the student record.");
                _response.ErrorMessages.Add(ex.Message); // Include the exception message for debugging
            }

            return _response;
        }
        



        public async Task<APIResponse> GetStudentRecord(string studentMatricNumber)
        {
            _logger.LogInformation("Fetching student records for {StudentMatricNumber}", studentMatricNumber);

            var studentRecords = await _studentRepository.GetAllAsync(studentMatricNumber);

            if (studentRecords == null)
            {
                _logger.LogWarning("Student records not found for {StudentMatricNumber}", studentMatricNumber);
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Student records not found.");
                return _response;
            }

            _logger.LogInformation("Student records fetched successfully for {StudentMatricNumber}", studentMatricNumber);
            _response.IsSuccess = true;
            _response.Result = studentRecords;
            return _response;
        }

        public Task<APIResponse> CheckResult(string studentMatricNumber)
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