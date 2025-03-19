using AutoMapper;
using Microsoft.EntityFrameworkCore;
using New_School_Management_API.Dbcontext;
using New_School_Management_API.Entities;
using SchoolManagmentAPI.StudentDTO;

using New_School_Management_API.Data;
using New_School_Management_API.DTO;
using New_School_Management_API.Repository;


namespace New_School_Management_API.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentManagementDB _dBContext;
        private readonly IMapper _mapper;
        private readonly APIResponse _aPIResponse = new APIResponse ();
        public StudentRepository(StudentManagementDB dBContext, IMapper mapper)
        {
            this._dBContext = dBContext;
            this._mapper = mapper;
        }

        public async Task<bool> AddAsync(StudentRecord studentClass)
        {
            await _dBContext.AddAsync(studentClass);
            await _dBContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GetAsync(string StudentMatricNumber)
        {
            var getStudentRecord = await _dBContext.StudentRecords.FindAsync(StudentMatricNumber);
            return getStudentRecord != null;
        }

        public async Task<List<GetStudentRecordDTO>> GetAllAsync(string studentMatricNumber)
        {
            if (string.IsNullOrEmpty(studentMatricNumber))
            {
                return new List<GetStudentRecordDTO>();
            }

            var studentClasses = await _dBContext.StudentRecords
                .Where(sc => sc.StudentMatricNumber == studentMatricNumber)
                .ToListAsync();

            return _mapper.Map<List<GetStudentRecordDTO>>(studentClasses);
        }





        public async Task<StudentRecord?> GetByMatericNumberAsync(string StudentMatricNumber)
        {
            if (string.IsNullOrWhiteSpace(StudentMatricNumber))
            {
                return null;
            }
            return await _dBContext.StudentRecords
                .FirstOrDefaultAsync(s => s.StudentMatricNumber == StudentMatricNumber);
        }




        public async Task<bool> UpdateAsync(StudentRecord updateStudentDTO)
        {
            var existingStudent = await _dBContext.StudentRecords.FindAsync(updateStudentDTO.StudentMatricNumber);
            if (existingStudent == null)
            {
                return false;
            }

            _mapper.Map(updateStudentDTO, existingStudent);
            _dBContext.StudentRecords.Update(existingStudent);
            await _dBContext.SaveChangesAsync();
            return true;
        }



        public async Task DeleteAsync(string studentMatricNumber)
        {
            var studentToDelete = await _dBContext.StudentRecords
                .FirstOrDefaultAsync(s => s.StudentMatricNumber == studentMatricNumber);

            if (studentToDelete == null)
                throw new KeyNotFoundException("Student not found.");

            _dBContext.StudentRecords.Remove(studentToDelete);
            await _dBContext.SaveChangesAsync();
        }



        public async Task<string> GetStudenResult(CheckResultDTO checkResult)
        {
            var student = await _dBContext.StudentRecords
                .FirstOrDefaultAsync(s => s.StudentMatricNumber == checkResult.StudentMatricNumber.ToString());

            if (student == null)
            {
                throw new Exception("Student Result not found in the DB. Kindly contact your Department or Visit ICT office.");
            }

            return "StudentResult Successful";
        }





        public async Task<bool> UserEmailAlreadyExist(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return false;
            }

            return await _dBContext.StudentRecords.AnyAsync(u => u.StudentEmail == Email);
        }




        public async Task<bool> GetUserNameAysnc(string Username)
        {
            var userExist = await _dBContext.StudentRecords.AnyAsync(u => u.StudentMatricNumber == Username);
            return !userExist;
        }

        public async Task<StudentRecord?> GetpasswordAsync(string passwordHash)
        {
            if (string.IsNullOrEmpty(passwordHash))
            {
                return null;
            }

            return await _dBContext.StudentRecords
                .SingleOrDefaultAsync(u => u.Password == passwordHash);
        }




        public async Task<bool> UserMatricNumberAlreadyExist(string studentMatricNumber)
        {
            return await _dBContext.StudentRecords.AnyAsync(u => u.StudentMatricNumber == studentMatricNumber);
        }

        //public async Task<StudentTimetable> FetchClassTimeTableAysnc(int CurrentLevel)
        //{
        //    var classTimeTable = await _dBContext.TimeTableDB
        //        .Where(u => u.Level == CurrentLevel)
        //        .ToListAsync();

        //    if (classTimeTable == null || !classTimeTable.Any())
        //    {
        //        return null;
        //    }

        //    return new StudentTimetable { ClassSchedule = classTimeTable };
        //}

        //public async Task<StudentTimetable> FetchExamTimeTableAysnc(int CurrentLevel)
        //{
        //    var examTimeTable = await _dBContext.ExamTimeTableDB
        //        .Where(u => u.Level == CurrentLevel)
        //        .ToListAsync();

        //    if (examTimeTable == null || !examTimeTable.Any())
        //    {
        //        return null;
        //    }

        //    return new StudentTimetable { ExamSchedule = examTimeTable };
        //}

        //public async Task<string> SeedTimeTableDataAsync(List<ScheduleItem> ClassTimetable, List<ScheduleItem> ExamTimeTable)
        //{
        //    bool dataUpdated = false;

        //    if (ClassTimetable != null && ClassTimetable.Any())
        //    {
        //        await _dBContext.TimeTableDB.AddRangeAsync(ClassTimetable);
        //        dataUpdated = true;
        //    }
        //    if (ExamTimeTable != null && ExamTimeTable.Any())
        //    {
        //        await _dBContext.ExamTimeTableDB.AddRangeAsync(ExamTimeTable);
        //        dataUpdated = true;
        //    }

        //    if (dataUpdated)
        //    {
        //        await _dBContext.SaveChangesAsync();
        //    }

        //    return "Time table updated Successfully.";
        //}




    }
}
