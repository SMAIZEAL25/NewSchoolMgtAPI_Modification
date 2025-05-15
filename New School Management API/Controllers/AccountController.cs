using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis.CSharp;
using New_School_Management_API.Data;
using New_School_Management_API.DTO;
using New_School_Management_API.EmailService;
using New_School_Management_API.ModelValidations;
using New_School_Management_API.StudentDTO;
using Microsoft.AspNetCore.Authentication.Cookies;



namespace New_School_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("UserBasedRateLimit")]

    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly IEmailService _emailService;

        public AccountController(IAuthManager authManager, IEmailService emailService)
        {
            _authManager = authManager;
            _emailService = emailService;
        }

        [HttpPost("Api/Register/Auth")]
        [ValidationModelState]

        public async Task<IActionResult> Register([FromBody] CreateStudentDTO createStudentDTO)
        {
            var result = await _authManager.Register(createStudentDTO);
            await _emailService.SendRegistrationSuccessEmailAsync(createStudentDTO.StudentEmailAddress, createStudentDTO.LastName);
            return Ok(result);
        }



        [HttpPost("API/Login/Auth")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var result = await _authManager.Login(loginDTO);
            var IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            await _emailService.SendLoginNotificationAsync(loginDTO.Email, IpAddress, DateTime.UtcNow);
            return Ok(result);
        }


        [HttpPost("user/logout/auth")]
        [Authorize (AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            // Sign out and delete the cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logout successful" });
        }
    }
}
