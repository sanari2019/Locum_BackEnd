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
    public class WardSupervisorController : ControllerBase
    {
        private readonly WardSupervisorRepository _wardRepository;

        public WardSupervisorController( WardSupervisorRepository wardRepository)
        {
            _wardRepository = wardRepository;
        }

        // [HttpGet]
        // public IActionResult GetDepartments()
        // {
        //     var departments = _wardRepository.GetDepartments();
        //     return Ok(departments);
        // }

        [HttpGet("{id}")]
        public IActionResult GetDepartment(int id)
        {
            var department = _wardRepository.GetDepartmentById(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }
          [HttpGet("{wardId}")]
        public IActionResult GetSupervisorByWardId(int wardId)
        {
            try
            {
                var supervisor = _wardRepository.GetSupervisorByWardId(wardId);
                if (supervisor == null)
                {
                    return NotFound();
                }
                return Ok(supervisor);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // [HttpPost]
        // public IActionResult CreateDepartment([FromBody] Department department)
        // {
        //     if (department == null)
        //     {
        //         return BadRequest();
        //     }
        //     _wardRepository.InsertDepartment(department);
        //     return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
        // }

        // Add other actions as needed
    }
}