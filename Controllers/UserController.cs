using Microsoft.AspNetCore.Mvc;
using Locum_Backend;
using Locum_Backend.Repositories;
using Locum_Backend.Models;
using Locum_Backend.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Text;
// using Microsoft.IdentityModel.Tokens;
// using System.IdentityModel.Tokens.Jwt;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;

namespace Locum_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly UsersRolesRepository _userRoleRepository;

        public UsersController(UserRepository userRepository, UsersRolesRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        // GET: api/users
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            return Ok(users);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userRepository.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // UsersController.cs

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            var user = _userRepository.GetUserByEmail(credentials.Email);

            if (user != null && EncDscPassword.EncryptPassword(credentials.Password) == user.Password)
            {
                // Authentication successful
                // Generate a JWT token or perform additional authentication steps here
                var token = GenerateJwtToken(user);

                // Return a success response
                return Ok(new { User = user, Token = token, Message = "Login successful" });
            }

            // Invalid credentials
            return Unauthorized(new { Message = "Invalid email or password" });
        }



        [HttpPost("login2")]
        public IActionResult Login2([FromBody] UserCredentials credentials)
        {
            var user = _userRepository.GetUserByUsername(credentials.Username);

            if (user != null && EncDscPassword.EncryptPassword(credentials.Password) == user.Password)
            {
                // Authentication successful
                // Generate a JWT token or perform additional authentication steps here
                var token = GenerateJwtToken(user);

                // Return a success response
                return Ok(new { User = user, Token = token, Message = "Login successful" });
            }

            // Invalid credentials
            return Unauthorized(new { Message = "Invalid email or password" });
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678$#@$^@1ERFa1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6")); // Replace with a secure secret key
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                // Add more claims as needed
            };

            var token = new JwtSecurityToken(
                issuer: "locum_backend",
                audience: "api/[controller]",
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Set the token expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // POST: api/users
        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {

            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                user.Username = user.First_Name + "." + user.Last_Name;
                user.Created_At = DateTime.Now;
                user.Password = EncDscPassword.EncryptPassword(user.Password);
                _userRepository.InsertUser(user);

                UsersRoles userRole = new UsersRoles
                {
                    User_Id = user.Id,
                    Employee_Type_Id = 1,
                    Created_At = DateTime.Now


                };
                _userRoleRepository.InsertUserRole(userRole);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
        }





        // PUT: api/users/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _userRepository.UpdateUser(user);
            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userRepository.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            _userRepository.DeleteUser(user);
            return NoContent();
        }

        // GET: api/users/{userId}/employee-type
        [HttpGet("{userId}/employee-type")]
        public IActionResult GetEmployeeTypeFromUserId(int userId)
        {
            try
            {
                var employeeType = _userRoleRepository.GetEmployeeTypeFromUserId(userId);

                if (employeeType != null)
                {
                    return Ok(employeeType);
                }

                return NotFound(new { Message = "Employee type not found for the specified user ID" });
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, new { Message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/users/{userId}/user-roles
        [HttpGet("{userId}/user-roles")]
        public IActionResult GetUserRolesByUserIdFromView(int userId)
        {
            try
            {
                var userRoles = _userRoleRepository.GetUserRolesByUserIdFromView(userId);

                if (userRoles != null && userRoles.Count > 0)
                {
                    return Ok(userRoles);
                }

                return NotFound(new { Message = "User roles not found for the specified user ID" });
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, new { Message = $"Internal server error: {ex.Message}" });
            }
        }
        [HttpGet("userrole/{user_id}")]
        public IActionResult GetUserRole(int user_id)
        {
            var userRole = _userRoleRepository.GetUserRoleByUserId(user_id);
            if (userRole == null)
            {
                return NotFound();
            }
            return Ok(userRole);
        }
    }
}
