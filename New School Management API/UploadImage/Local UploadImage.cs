using New_School_Management_API.Dbcontext;
using New_School_Management_API.Entities;
using New_School_Management_API.Data;
using New_School_Management_API.Repository;
using System.Net;

namespace New_School_Management_API.UploadImage
{
    public class UploadImage : IUploadImage
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly StudentManagementDB studentManagementDB;
        private readonly ILogger<UploadImage> _logger;
    

        public UploadImage(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            StudentManagementDB studentManagementDB, ILogger<UploadImage> logger
            )
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.studentManagementDB = studentManagementDB;
            this._logger = logger;
            
        }



        //public async Task<APIResponse> UploadFile( Upload upload)
        //{
          

        //    var response = new APIResponse { IsSuccess = false };

        //    try
        //    {
        //    //    // Retrieve the student by matric number
        //    //    var student = await _studentRepository.GetByMatericNumberAsync(studentMatricnumber);
        //    //    if (student == null)
        //    //    {
        //    //        response.ErrorMessages = new List<string>
        //    //{
        //    //    "Student record not found",
        //    //    "Kindly ensure you've completed your registration",
        //    //};
        //    //        response.StatusCode = HttpStatusCode.NotFound; // Not Found
        //    //        return response;
        //    //    }

        //        // Validate file
        //        var allowedExtensions = new[] { ".jpg", ".jpeg", ".pdf" };
        //        var fileExtension = Path.GetExtension(upload.FileName).ToLowerInvariant();

        //        if (!allowedExtensions.Contains(fileExtension))
        //        {
        //            _logger.LogWarning("Invalid file extension: {FileExtension}", fileExtension);
        //            response.ErrorMessages.Add("Only JPG, JPEG, and PDF file types are allowed.");
        //            response.StatusCode = HttpStatusCode.NotAcceptable; // Bad Request
        //            return response;
        //        }

        //        const long MaxJpgFileSize = 5 * 1024 * 1024; // 5 MB
        //        if ((fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".pdf") && upload.FileSizeInBytes > MaxJpgFileSize)
        //        {
        //            _logger.LogWarning($"File size exceeds limit: {upload.FileSizeInBytes}");
        //            response.ErrorMessages.Add("JPG files must be smaller than 5 MB.");
        //            response.StatusCode = HttpStatusCode.BadRequest; // Bad Request
        //            return response;
        //        }

        //        // Read file content into byte array
        //        byte[] fileBytes;
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await upload.file.CopyToAsync(memoryStream);
        //            fileBytes = memoryStream.ToArray();
        //        }

        //        // Prepare the Upload object
        //        var uploadFile = new Upload
        //        {
                    
        //            FileName = upload.FileName,
        //            fileExtension = fileExtension,
        //            FileSizeInBytes = upload.FileSizeInBytes,
        //            FileDescription = fileBytes,
        //            UploadedOn = DateTime.UtcNow,
        //            file = upload.file // Ensure the file is included for UploadToFolder
        //        };

        //        // Save file to folder and update file path
        //        var uploadedFile = await UploadToFolder(uploadFile);

        //        // Save file metadata to database
        //        var fileId = await SaveFileAsync(uploadedFile);
                
        //        // Return success response
        //        response.IsSuccess = true;
        //        response.StatusCode = HttpStatusCode.OK; // OK
        //        response.Result = new { fileId, Message = "File uploaded successfully." };
        //    }
        //    catch (Exception ex)
        //    {
              
        //        response.ErrorMessages = new List<string> { "An unexpected error occurred. Please try again later." };
        //        response.StatusCode = HttpStatusCode.InternalServerError; // Internal Server Error
        //    }

        //    return response;
        //}



        public async Task<Upload> UploadToFolder(Upload image)
        {

            // Extract the file extension
            image.FileExtension = Path.GetExtension(image.file.FileName);


            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "uploadImageFloder",
                $"{image.FileName}{image.FileExtension}");
          
            //upload image to local path 
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.file.CopyToAsync(stream);

            // https//localhost:1233/image/emage.jpg

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}" +
                $"://{httpContextAccessor.HttpContext.Request.Host}" +
                $"{httpContextAccessor.HttpContext.Request.PathBase}" +
                $"/UploadImageFloder/{image.FileName}{image.FileExtension}";

            // feeding the file path property in the filepath

            image.FilePath = urlFilePath;

            // Add image to the image table 
            await studentManagementDB.Uploads.AddAsync(image);
            await studentManagementDB.SaveChangesAsync();

            return image;

        }

        public Task<Upload?> GetFileAsync(int studentmatricnumber)
        {
            throw new NotImplementedException();
        }

        public Task<APIResponse<object>> SaveFileAsync(Upload upload)
        {
            throw new NotImplementedException();
        }
    }
}
