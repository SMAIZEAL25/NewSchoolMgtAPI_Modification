using AutoMapper;
using New_School_Management_API.Domain.Entities;
using New_School_Management_API.Domain.StudentDTO;
using System.ComponentModel.Design;

namespace New_School_Management_API.Domain.MapConfig
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<StudentRecord, GetStudentRecordDTO>().ReverseMap();
            // Reversed Mapping 
            CreateMap<StudentRecord, CreateStudentDTO>().ReverseMap();
            CreateMap<UpdateStudentDTO, StudentRecord>().ReverseMap();
            CreateMap<StudentRecord, CheckoutException>().ReverseMap();
            CreateMap<Upload, UploadFileDTO>().ReverseMap();
            CreateMap<StudentRecord, LoginDTO>().ReverseMap();

        }
    }
}
