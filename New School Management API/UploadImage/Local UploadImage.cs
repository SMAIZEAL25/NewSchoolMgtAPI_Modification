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
        private readonly IStudentRepository _studentRepository;

        public UploadImage(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            StudentManagementDB studentManagementDB, ILogger<UploadImage> logger,
            IStudentRepository studentRepository)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.studentManagementDB = studentManagementDB;
            this._logger = logger;
            this._studentRepository = studentRepository;
        }



        public async Task<APIResponse> UploadFile(string studentMatricnumber, Upload upload)
        {
            _logger.LogInformation("Uploading file for student {StudentMatricNumber}", studentMatricnumber);

            var response = new APIResponse { IsSuccess = false };

            try
            {
                // Retrieve the student by matric number
                var student = await _studentRepository.GetByMatericNumberAsync(studentMatricnumber);
                if (student == null)
                {
                    _logger.LogWarning("Student record not found for {StudentMatricNumber}", studentMatricnumber);
                    response.ErrorMessages = new List<string>
            {
                "Student record not found",
                "Kindly ensure you've completed your registration",
            };
                    response.StatusCode = HttpStatusCode.NotFound; // Not Found
                    return response;
                }

                // Validate file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".pdf" };
                var fileExtension = Path.GetExtension(upload.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    _logger.LogWarning("Invalid file extension: {FileExtension}", fileExtension);
                    response.ErrorMessages.Add("Only JPG, JPEG, and PDF file types are allowed.");
                    response.StatusCode = HttpStatusCode.BadRequest; // Bad Request
                    return response;
                }

                const long MaxJpgFileSize = 5 * 1024 * 1024; // 5 MB
                if ((fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".pdf") && upload.FileSizeInBytes > MaxJpgFileSize)
                {
                    _logger.LogWarning("File size exceeds limit: {FileSize}", upload.FileSizeInBytes);
                    response.ErrorMessages.Add("JPG files must be smaller than 5 MB.");
                    response.StatusCode = HttpStatusCode.BadRequest; // Bad Request
                    return response;
                }

                // Read file content into byte array
                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await upload.file.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                // Prepare the Upload object
                var uploadFile = new Upload
                {
                    StudentId = student.Id, // Assign the StudentId (int) from the retrieved student
                    FileName = upload.FileName,
                    fileExtension = fileExtension,
                    FileSizeInBytes = upload.FileSizeInBytes,
                    FileDescription = fileBytes,
                    UploadedOn = DateTime.UtcNow,
                    file = upload.file // Ensure the file is included for UploadToFolder
                };

                // Save file to folder and update file path
                var uploadedFile = await UploadToFolder(uploadFile);

                // Save file metadata to database
                var fileId = await SaveFileAsync(uploadedFile);
                _logger.LogInformation("File uploaded successfully for student {StudentMatricNumber}", studentMatricnumber);

                // Return success response
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK; // OK
                response.Result = new { fileId, Message = "File uploaded successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading the file for student {StudentMatricNumber}", studentMatricnumber);
                response.ErrorMessages = new List<string> { "An unexpected error occurred. Please try again later." };
                response.StatusCode = HttpStatusCode.InternalServerError; // Internal Server Error
            }

            return response;
        }



        public async Task<Upload> UploadToFolder(Upload image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "uploadImagesFloder",
                $"{image.FileName}{image.fileExtension}");


            //upload image to local path 
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.file.CopyToAsync(stream);

            // https//localhost:1233/image/emage.jpg

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}" +
                $"://{httpContextAccessor.HttpContext.Request.Host}" +
                $"{httpContextAccessor.HttpContext.Request.PathBase}" +
                $"/UploadImagesFloder/{image.FileName}{image.fileExtension}";

            // feeding the file path property in the filepath

            image.filePath = urlFilePath;

            // Add image to the image table 
            await studentManagementDB.AddAsync(image);
            await studentManagementDB.SaveChangesAsync();

            return image;

        }

        public Task<Upload?> GetFileAsync(int studentmatricnumber)
        {
            throw new NotImplementedException();
        }

        public Task<APIResponse> SaveFileAsync(Upload upload)
        {
            throw new NotImplementedException();
        }
    }
}
