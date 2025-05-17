using System.Drawing;
using System.Net;

namespace New_School_Management_API.Data
{
    public class APIResponse <T>
    {

        // here is the API response implemented in the controller class 
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public string Token { get; set; }
        public DateTime? ExpiresIn { get; set; } // Nullable to indicate when not used
        public CookieSettings CookieSettings { get; set; }
        public T Data { get; set; } // Added for completeness, as used in previous responses


        // Optional: Control serialization to exclude null properties
        public bool ShouldSerializeToken() => Token != null;
        public bool ShouldSerializeExpiresIn() => ExpiresIn.HasValue;
        public bool ShouldSerializeCookieSettings() => CookieSettings != null;
        public bool ShouldSerializeData() => Data != null;


    }


}
