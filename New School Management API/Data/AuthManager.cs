﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using New_School_Management_API.DTO;
using New_School_Management_API.Entities;
using New_School_Management_API.Repository;
using New_School_Management_API.StudentDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace New_School_Management_API.Data
{
    public class AuthManager : IAuthManager
    {
        private readonly APIResponse<object> _response = new APIResponse<object>();
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStudentRepository _studentRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManager> _logger;

        public AuthManager(
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            IStudentRepository studentRepository,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ILogger<AuthManager> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _studentRepository = studentRepository;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<APIResponse<object>> Register (CreateStudentDTO createStudentDTO)
        {
            var response = new APIResponse<object>
            {
                ErrorMessages = new List<string>()
            };

            _logger.LogInformation($"Registering user with email {createStudentDTO.StudentEmailAddress}");

        
            var userExists = await _userManager.FindByEmailAsync(createStudentDTO.StudentEmailAddress);
            if (userExists != null)
            {
                _logger.LogWarning($"User already exists with email {createStudentDTO.StudentEmailAddress}");
                response.IsSuccess = false;
                response.ErrorMessages.Add("User already exists with this email.");
                return response;
            }

         
            var passwordValidationMessage = IsValidPassword(createStudentDTO.Password);
            if (!string.IsNullOrEmpty(passwordValidationMessage))
            {
                _logger.LogWarning($"Password does not meet requirements for email {createStudentDTO.StudentEmailAddress}");
                response.IsSuccess = false;
                response.ErrorMessages.Add(passwordValidationMessage);
                return response;
            }

           
            var identityUser = new IdentityUser
            {
                UserName = createStudentDTO.LastName, 
                Email = createStudentDTO.StudentEmailAddress,
                PasswordHash = createStudentDTO.Password
            };

            var creationResult = await _userManager.CreateAsync(identityUser);
            if (!creationResult.Succeeded)
            {
                _logger.LogWarning($"Failed to create user with email {createStudentDTO.StudentEmailAddress}");
                response.IsSuccess = false;
                response.ErrorMessages.AddRange(creationResult.Errors.Select(e => e.Description));
                return response;
            }

            var roleExists = await _roleManager.RoleExistsAsync(createStudentDTO.Roles);
            if (!roleExists)
            {
                var roleCreationResult = await _roleManager.CreateAsync(new IdentityRole(createStudentDTO.Roles));
                if (!roleCreationResult.Succeeded)
                {
                    _logger.LogWarning($"Failed to create role {createStudentDTO.Roles}");
                    response.IsSuccess = false;
                    response.ErrorMessages.Add("Failed to create the role. Please try again.");
                    return response;
                }
            }

          
            var addToRoleResult = await _userManager.AddToRoleAsync(identityUser, createStudentDTO.Roles);
            if (!addToRoleResult.Succeeded)
            {
                _logger.LogWarning($"Failed to assign role {createStudentDTO.Roles} to user {createStudentDTO.StudentEmailAddress}");
                response.IsSuccess = false;
                response.ErrorMessages.Add("Failed to assign the user to the role.");
                return response;
            }

            
            try
            {
                
                var matricNumber = await GenerateMatricNumberAsync(createStudentDTO.Faculty, createStudentDTO.Department);
                createStudentDTO.StudentMatricNumber = matricNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error generating matric number: {ex.Message}");
                response.IsSuccess = false;
                response.ErrorMessages.Add("Failed to generate matric number. Please ensure faculty and department are provided.");
                return response;
            }

          
            var studentRecord = _mapper.Map<StudentRecord>(createStudentDTO);
            studentRecord.IdentityUserId = identityUser.Id; // Link IdentityUser with StudentRecord

            // Save student record to database
            try
            {
                var studentSaveResult = await _studentRepository.creataStudentRecord(studentRecord);
                if (!studentSaveResult)
                {
                    _logger.LogWarning($"Failed to save student details for email {createStudentDTO.StudentEmailAddress}");
                    response.IsSuccess = false;
                    response.ErrorMessages.Add("Failed to save student details. Please try again.");
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while saving student data: {ex.Message}");
                response.IsSuccess = false;
                response.ErrorMessages.Add("An error occurred while saving student details.");
                return response;
            }

            _logger.LogInformation($"User registered successfully with email {createStudentDTO.StudentEmailAddress}");
            response.IsSuccess = true;
            response.Message = "Registration successful with matric number " + createStudentDTO.StudentMatricNumber;
            return response;
        }


        private string IsValidPassword(string password)
        {
            // Ensure password has at least one uppercase letter, one digit, and one special character
            if (string.IsNullOrWhiteSpace(password))
                return "Password is required";

            if (password.Length < 8)
                return "Password must be at least 8 characters";

            if (!password.Any(char.IsUpper))
                return "Password must contain at least one uppercase letter";

            if (!password.Any(char.IsLower))
                return "Password must contain at least one lowercase letter";

            if (!password.Any(char.IsDigit))
                return "Password must contain at least one digit";

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                return "Password must contain at least one special character";


            var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+={}\[\]:;'<>?,./\\-]).+$");
            if (!regex.IsMatch(password))
            {
                return "Password must contain at least one uppercase letter, one number, and one special character.";
            }
            return string.Empty; // Valid password
        }

        // generate matric number method 
        // Get the current year
        // Get the first 3 letters (uppercase) or the full string if less than 3 character
        // Increment the sequence number
        // Format the sequence number to always have 3 digits (e.g., 001, 002, etc.)
        // Concatenate the parts to form the matric number
        // Retrieve the last sequential number for the given year, department, and faculty from your repository or DB.
        // For example, this method should return an integer representing the last number generated for that group.
        // Return the matric number in the format: DEPT/FAC/Year/Sequence
        private async Task<string> GenerateMatricNumberAsync(string? faculty, string? department)
        {

            string deptCode = department.Length >= 3 ? department.Substring(0, 3).ToUpper() : department.ToUpper();
            string facCode = faculty.Length >= 3 ? faculty.Substring(0, 3).ToUpper() : faculty.ToUpper();
            string year = DateTime.UtcNow.Year.ToString();

            var lastMatricNumber = await _studentRepository.GetLastMatricNumber(deptCode, facCode, year);

            int newSequentialNumber = 1;
            if (!string.IsNullOrEmpty(lastMatricNumber))
            {
                
                var parts = lastMatricNumber.Split('/');
                if (parts.Length == 4 && int.TryParse(parts[3], out int lastNumber))
                {
                    newSequentialNumber = lastNumber + 1;
                }
            } 
            string seqFormatted = newSequentialNumber.ToString("001");

            return $"{deptCode}/{facCode}/{year}/{seqFormatted}";
        }


        // Login Method 
        public async Task<APIResponse<object>> Login(LoginDTO loginDTO)
        {
            var response = new APIResponse<object>
            {
                ErrorMessages = new List<string>()
            };

            _logger.LogInformation("Logging in user with email {Email}", loginDTO.Email);

            // Find user by email
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                _logger.LogWarning("User not found with email {Email}", loginDTO.Email);
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.ErrorMessages.Add("Invalid email or password.");
                return response;
            }

            // Verify password
            if (!await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                _logger.LogWarning("Invalid password for email {Email}", loginDTO.Email);
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.Unauthorized;
                response.ErrorMessages.Add("Invalid email or password.");
                return response;
            }

            // Check roles
            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            if (!roles.Any(r => r == "Reader" || r == "Writer"))
            {
                _logger.LogWarning("User does not have the required role for email {Email}", loginDTO.Email);
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.Forbidden;
                response.ErrorMessages.Add("User does not have required role.");
                return response;
            }

            // Generate a JWT token
            var jwtResult = await GenerateJwtToken(user, roles);
            if (jwtResult == null)
            {
                _logger.LogError("Error generating token for email {Email}", loginDTO.Email);
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add("An error occurred during authentication.");
                return response;
            }

            _logger.LogInformation("User logged in successfully with email {Email}", loginDTO.Email);

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Login successful.";
            response.ExpiresIn = jwtResult.ExpiresIn;
            response.Token = jwtResult.Token;
            //response.UserId = jwtResult.UserId;

            return response;
        }

        private async Task<APIResponse<object>> GenerateJwtToken(IdentityUser user, List<string> roles)
        {
            var response = new APIResponse<object>
            {
                ErrorMessages = new List<string>()
            };

            try
            {
                _logger.LogInformation("Generating JWT token for user {UserId}", user.Id);

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var userClaims = await _userManager.GetClaimsAsync(user);
                var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("uid", user.Id),
                }
                .Union(userClaims)
                .Union(roleClaims);

                var tokenDuration = Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"]);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(tokenDuration),
                    signingCredentials: credentials
                );

                _logger.LogInformation($"JWT token generated successfully for user {user.Id}");

                response.Token = new JwtSecurityTokenHandler().WriteToken(token);
                response.ExpiresIn = token.ValidTo;
                //response.UserId = user.Id;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token for user {UserId}", user.Id);
                response.IsSuccess = false;
                response.ErrorMessages.Add("An error occurred while generating the token.");
                return response;
            }
        }
    }
}
