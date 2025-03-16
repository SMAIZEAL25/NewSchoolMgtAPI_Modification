namespace New_School_Management_API.StudentDTO
{
    public class UploadFileDTO
    {
        public string FileName { get; set; }
        public string FileType { get; set; } //  aspect only "JPG" or "PDF"
        public long FileSize { get; set; }   // File size in bytes
        public byte[] FileContent { get; set; }
        public DateTime UploadedOn { get; set; }
    }
}
