using Microsoft.AspNetCore.Mvc;
using Locum_Backend;
using Locum_Backend.Repositories;
using Locum_Backend.Models;
using Locum_Backend.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Text;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;

namespace Locum_Backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ValidatedPatientController : ControllerBase
    {
        private readonly ValidatedPatientRepository _validatedpatienttRepository;
        private readonly PatientRepository _patientRepository;
        private readonly ApprovalRequestRepository _approvalRequest;
        private readonly UserRepository _userRequest;

        public ValidatedPatientController(ValidatedPatientRepository validatedpatientRepository, PatientRepository patientRepository, ApprovalRequestRepository approvalRequest, UserRepository userRequest)
        {
            _validatedpatienttRepository = validatedpatientRepository;
            _patientRepository = patientRepository;
            _approvalRequest = approvalRequest;
            _userRequest = userRequest;
        }

        // [HttpGet]
        // public IActionResult GetDepartments()
        // {
        //     // var departments = _validatedpatienttRepository.GetValidatedPatient();
        //     // return Ok(departments);
        //     return NoContent();
        // }

        // [HttpGet]
        // public IActionResult GetValidatedPatient([FromBody] NursedPatient np)
        // {
        //     var valpatient = _validatedpatienttRepository.GetValidatedPatient(np);
        //     if (valpatient == null)
        //     {
        //         return NotFound();
        //     }
        //     return Ok(valpatient);
        // }

        [HttpPost]
        public IActionResult GetValidatedPatient([FromBody] NursedPatient np)
        {
            try
            {
                var valPatient = _validatedpatienttRepository.GetValidatedPatient(np);
                if (valPatient == null)
                {
                    return NotFound();
                }
                return Ok(valPatient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // [HttpPost]
        // public IActionResult CreateDepartment([FromBody] Department department)
        // {
        //     if (department == null)
        //     {
        //         return BadRequest();
        //     }
        //     _departmentRepository.InsertDepartment(department);
        //     return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
        // }

        // Add other actions as needed
    }
}
