using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Azure;
using New_School_Management_API.Domain.Data;
using New_School_Management_API.Domain.StudentDTO;
using New_School_Management_API.Domain.ModelValidations;



namespace New_School_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("UserBasedRateLimit")]

    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        

        public AccountController(IAuthManager authManager  )
        {
            _authManager = authManager;
        }

        [HttpPost("Api/Auth/Register")]
        [ValidationModelState]
        public async Task<IActionResult> Register([FromBody] CreateStudentDTO createStudentDTO)
        {
            var result = await _authManager.Register(createStudentDTO);
            return Ok(result);
        }



        [HttpPost("API/Auth/Login")]
        [ValidationModelState]        
        
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var result = await _authManager.Login(loginDTO);            
            return Ok(result);
        }


        [HttpPost("user/Auth/LogOut")]        
        public async Task<IActionResult> Logout()
        {
            // Do not use the cookiew authentication scheme here if you are using JWT Bearer tokens for authentication use the Jwt to sign out
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);           
            return Ok(new { message = "Logout successful" });
        }
    }
}
