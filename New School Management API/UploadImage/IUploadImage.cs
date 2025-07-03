using New_School_Management_API.Data;
using New_School_Management_API.Domain.Entities;

namespace New_School_Management_API.UploadImage
{
    public interface IUploadImage
    {
        //Task<APIResponse> UploadFile(Upload upload);
        //Task <APIResponse> SaveFileAsync (Upload upload);
        //Task<Upload?> GetFileAsync(int studentmatricnumber);
        Task<Upload> UploadToFolder(Upload upload);
    }
}
