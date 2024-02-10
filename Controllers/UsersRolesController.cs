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
    public class UsersRolesController : ControllerBase
    {
        private readonly UsersRolesRepository _userRepository;
        // private readonly UserRoleRepository _userRoleRepository;

        public UsersRolesController(UsersRolesRepository userRepository)
        {
            _userRepository = userRepository;
            // _userRoleRepository = userRoleRepository;
        }

        [HttpGet]
        public IActionResult GetUserRoles()
        {
            var userRoles = _userRepository.GetUserRoles();
            return Ok(userRoles);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserRole(int id)
        {
            var userRole = _userRepository.GetUserRoleById(id);
            if (userRole == null)
            {
                return NotFound();
            }
            return Ok(userRole);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateUserRole(int id, [FromBody] UsersRoles userRole)
        {
            if (userRole == null || id != userRole.Id)
            {
                return BadRequest();
            }

            var existingUserRole = _userRepository.GetUserRoleById(id);
            if (existingUserRole == null)
            {
                return NotFound();
            }

            _userRepository.UpdateUserRole(userRole);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserRole(int id)
        {
            var userRole = _userRepository.GetUserRoleById(id);
            if (userRole == null)
            {
                return NotFound();
            }

            _userRepository.DeleteUserRole(userRole);
            return NoContent();
        }


        [HttpPost]
        public IActionResult CreateUserRole([FromBody] UsersRoles usersRoles)
        {

            if (usersRoles == null)
            {
                return BadRequest();
            }
            else
            {

                _userRepository.InsertUserRole(usersRoles);

                // UsersRoles userRole = new UsersRoles
                // {
                //     UserId = user.Id,
                //     EmployeeTypeId = 1,
                //     CreatedAt = DateTime.Now


                // };
                // _userRoleRepository.InsertUserRole(userRole);
                return Ok(usersRoles);
            }
        }





        // PUT: api/users/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UsersRoles user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _userRepository.UpdateUserRole(user);
            return NoContent();
        }



    }
}
