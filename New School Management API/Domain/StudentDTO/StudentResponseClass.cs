﻿namespace New_School_Management_API.Domain.StudentDTO
{
    public class StudentResponseClass
    {
        public string SurName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public int CurrentLevel { get; set; }
        public string StudentMatricNumber { get; set; }
        public decimal GPA { get; set; }
    }
}
