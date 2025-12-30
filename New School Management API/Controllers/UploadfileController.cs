using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using New_School_Management_API.Domain.Entities;
using New_School_Management_API.Domain.StudentDTO;
using New_School_Management_API.UploadImage;

namespace New_School_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UploadfileController : ControllerBase
    {
        private readonly IUploadImage _uploadImage;
        private readonly IMapper _mapper;

        public UploadfileController(IUploadImage uploadImage, IMapper mapper)
        {
            _uploadImage = uploadImage;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles([FromForm] UploadFileDTO uploadFileDTO)
        {
            // Validate the file upload
            ValidateFileUpload(uploadFileDTO);

            // Check if the model state is valid after validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mappermodel = _mapper.Map<Upload>(uploadFileDTO);
            // Upload the file
            var response = await _uploadImage.UploadToFolder(mappermodel);

            // Return the response from the upload service
            return Ok(new { message = "Image Uploaded Successfully", filePath = response });
        }

        private void ValidateFileUpload(UploadFileDTO uploadFileDTO)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(uploadFileDTO.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (uploadFileDTO.File.Length > 10485760) // 10MB
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file");
            }
        }
    }
}