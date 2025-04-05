using System.Net;

namespace New_School_Management_API.Data
{
    public class APIResponse <T>
    {


        // here is the API response implemented in the controller class 
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; }
        public object Data { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public object Result { get; set; }



    }
}
