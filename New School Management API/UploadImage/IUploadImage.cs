using New_School_Management_API.Entities;
using New_School_Management_API.Data;

namespace New_School_Management_API.UploadImage
{
    public interface IUploadImage
    {
        Task<APIResponse> UploadFile(string studentMatricnumber, Upload upload);
        Task <APIResponse> SaveFileAsync (Upload upload);
        Task<Upload?> GetFileAsync(int studentmatricnumber);
        Task<Upload> UploadToFolder(Upload image);
    }
}
