﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using New_School_Management_API.Data;
using New_School_Management_API.DTO;
using New_School_Management_API.ModelValidations;
using New_School_Management_API.StudentDTO;

namespace New_School_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AccountController(IAuthManager authManager)
        {
            _authManager = authManager; 
        }

        [HttpPost("Api/Register/Auth")]
        [ValidationModelState]

        public async Task<IActionResult> Register([FromBody] CreateStudentDTO createStudentDTO)
        {
            await _authManager.Register(createStudentDTO);
            return Ok();

        }

        [HttpPost("API/Login/Auth")]
        public async Task <IActionResult> Login (LoginDTO loginDTO)
        {
            await _authManager.Login(loginDTO);
            return Ok();
        }

    }
}
